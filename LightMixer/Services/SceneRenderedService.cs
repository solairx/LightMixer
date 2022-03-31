using System;
using System.Threading;
using Microsoft.Practices.Unity;
using LightMixer.Model.Fixture;
using System.Collections.Concurrent;
using BeatDetector;
using System.Collections.Generic;
using System.Linq;
using static LightMixer.Model.DmxChaser;
using Microsoft.Practices.ObjectBuilder2;
using System.Diagnostics;
using LightMixer.View;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model
{
    public class SceneRenderedService
    {
        public readonly SceneService sceneService;
        private readonly DmxChaser legacyChaser;
        private readonly VComWrapper dmxWrapper;
        private readonly VirtualDjServer vdjServer;
        private bool isRunning = true;
        private Thread runningThread;
        private DmxEffectSelector DmxEffectSelector ;
        private ConcurrentDictionary<int, VdjEvent> LastVdjEvent = new ConcurrentDictionary<int, VdjEvent>();
        private ActiveDeckSelector ActiveDeckSelector;
        private int FrameRate = 25; //25 ms
        private int UpdateUiRate = 4; // update every 4*25 ms (100ms )
        private int UpdateColorRate = 8; // update every 4*25 ms (100ms )
        private int UpdateWebDeviceRate = 2; // update every 4*25 ms (100ms )
        private OS2lEvent LastOs2lEvent = new OS2lEvent { };

        public SceneRenderedService(SceneService sceneService, DmxChaser legacyChaser, VComWrapper dmxWrapper, VirtualDjServer vdjServer)
        {
            DmxEffectSelector = new DmxEffectSelector(this);
            ActiveDeckSelector = new ActiveDeckSelector();
            LastVdjEvent[1] = new VdjEvent();
            LastVdjEvent[2] = new VdjEvent();
            LastVdjEvent[3] = new VdjEvent();
            LastVdjEvent[4] = new VdjEvent();
            this.sceneService = sceneService;
            this.legacyChaser = legacyChaser;
            this.dmxWrapper = dmxWrapper;
            this.vdjServer = vdjServer;
            runningThread = new Thread(new ThreadStart(Run));
        //    runningThread.IsBackground = true;
            runningThread.Start();
            if (vdjServer != null)
            {
                vdjServer.VirtualDjServerEvent += VirtualDjServer_VirtualDjServerEvent;
                vdjServer.OS2lServerEvent += VdjServer_OS2lServerEvent;
            }
        }

        

        private void Run()
        {
            while (!MainWindow.IsDead)
            {
                var sw = new Stopwatch();
                try
                {
                    var activeDeck = ActiveDeckSelector.Select(LastVdjEvent.Values);
                    UpdateUiRate++;
                    if (UpdateUiRate > 4)
                    {
                        UpdateUiRate = 0;
                        legacyChaser.UpdateVDJUiElement(activeDeck);
                        if (legacyChaser.POIs?.Any() == false)
                        {
                            vdjServer.vdjDataBase.CheckForRefresh();
                        }
                    }

                    DmxEffectSelector.Select(legacyChaser, activeDeck);

                    sceneService.Scenes
                        .SelectMany(o => o.Zones)
                        .SelectMany(o => o.FixtureTypes)
                        .ForEach(o => o.CurrentEffect.DmxFrameCall(activeDeck));

                    var allfixture = sceneService.Scenes
                        .SelectMany(o => o.Zones)
                        .SelectMany(o => o.FixtureTypes)
                        .SelectMany(o => o.FixtureGroups)
                        .SelectMany(o => o.FixtureInGroup);
                    byte?[] dmxFrameArray = RenderDMXFrame(allfixture);
                    if (legacyChaser.LedEffectCollection.Count != 0)
                    {
                        /*if (legacyChaser.LedEffectCollection[0]._sharedEffectModel.AutoChangeColorOnBeat && UpdateColorRate == 0)
                        {
                            // because we want to rotate more color :D
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                        }
                        UpdateColorRate++;
                        if (UpdateColorRate > 8)
                            UpdateColorRate = 0;*/
                        if (legacyChaser.LedEffectCollection[0]._sharedEffectModel.AutoChangeColorOnBeat)
                        {
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                        }
                    }

                    for (int position = 0; position < dmxFrameArray.Length; position++)
                    {
                        if (dmxWrapper.Buffer[position] != dmxFrameArray[position])
                        {
                            dmxWrapper.Buffer = ConvertByteArray(dmxFrameArray);
                            break;
                        }
                    }
                }
                catch (Exception vexp)
                {
                    Debug.WriteLine(vexp.ToString());
                }
          //      Debug.WriteLine("FRAME TIME : " + sw.ElapsedMilliseconds);
                Thread.Sleep(FrameRate);
            }
        }

        private byte?[] RenderDMXFrame(IEnumerable<FixtureBase> fixtures)
        {
            byte?[] array = new byte?[512];

            foreach (FixtureBase fixture in fixtures.Where(f=>f.IsRenderOnDmx))
            {

                byte?[] fixtureValue = fixture.Render();

                int x = 0;
                for (x = fixture.StartDmxAddress; x < fixture.StartDmxAddress + fixture.DmxLenght; x++)
                {
                    if (fixtureValue[x].HasValue)
                        array[x] = fixtureValue[x].Value;
                }
            }
            foreach (FixtureBase fixture in fixtures.Where(f => !f.IsRenderOnDmx))
            {
                byte?[] fixtureValue = fixture.Render();
            }

            foreach (var renderer in fixtures.Where(f => f.HttpMulticastRenderer !=null)
                .Select(f=>f.HttpMulticastRenderer)
                .Distinct())
            {
                renderer.Render();
            }
            return array;
        }

        

        private void VdjServer_OS2lServerEvent(OS2lEvent os2lEvent)
        {
            LastOs2lEvent = os2lEvent;
            LastVdjEvent[1].BeatPos = LastOs2lEvent.BeatPos;
            LastVdjEvent[2].BeatPos = LastOs2lEvent.BeatPos;
        }
        private void VirtualDjServer_VirtualDjServerEvent(VdjEvent vdjEvent)
        {
            if (LastOs2lEvent.BeatPos != 0)
            {
                vdjEvent.BeatPos = LastOs2lEvent.BeatPos;
            }
            LastVdjEvent[vdjEvent.Deck] = vdjEvent;
        }

        private byte[] ConvertByteArray(byte?[] arrayToConvert)
        {
            byte[] res = new byte[arrayToConvert.Length];
            int x = 0;
            for (x = 0; x < arrayToConvert.Length; x++)
            {
                if (arrayToConvert[x].HasValue)
                    res[x] = arrayToConvert[x].Value;
            }
            return res;
        }

        public EffectBase GetCurrentEffect<T>(string scene, string zone) where T : FixtureCollection
        {
            return GetCurrentFixture<T>(scene, zone)
                .First().CurrentEffect;
        }

        public IEnumerable<FixtureCollection> GetCurrentFixture<T>(string scene, string zone) where T : FixtureCollection
        {
            return sceneService.Scenes
                            .First(o => o.Name == scene)
                            .Zones.Where(z => z.Name == zone)
                            .First().FixtureTypes.OfType<T>();
        }

        public void SetCurrentEffect<T>(string scene, string zone, EffectBase newEffect) where T : FixtureCollection
        {
            var selectedZone =
            sceneService.Scenes
            .First(o => o.Name == scene)
            .Zones.Where(z => z.Name == zone);


            selectedZone.Single().FixtureTypes.OfType<T>()
            .First().CurrentEffect = newEffect;
        }

        public void SetMovingHeadProgramEffect(string scene, string zone, Program newProgram) 
        {
            var selectedZone =
            sceneService.Scenes
            .First(o => o.Name == scene)
            .Zones.Where(z => z.Name == zone);
            
            selectedZone.Single().FixtureTypes.OfType<MovingHeadFixtureCollection>()
            .First()
            .FixtureGroups
            .SelectMany(o=>o.FixtureInGroup)
            .OfType<MovingHeadFixture>()
            .ForEach(mh=>mh.ProgramMode = newProgram);
        }

        public void SetMovingHeadAlternateColor(string scene, string zone, bool newProgram)
        {
            var selectedZone =
            sceneService.Scenes
            .First(o => o.Name == scene)
            .Zones.Where(z => z.Name == zone);

            selectedZone.Single().FixtureTypes.OfType<MovingHeadFixtureCollection>()
            .First()
            .FixtureGroups
            .SelectMany(o => o.FixtureInGroup)
            .OfType<MovingHeadFixture>()
            .ForEach(mh => mh.EnableAlternateColor = newProgram);
        }

        public void SetMovingHeadDelayedPosition(string scene, string zone, bool newProgram)
        {
            var selectedZone =
            sceneService.Scenes
            .First(o => o.Name == scene)
            .Zones.Where(z => z.Name == zone);

            selectedZone.Single().FixtureTypes.OfType<MovingHeadFixtureCollection>()
            .First()
            .FixtureGroups
            .SelectMany(o => o.FixtureInGroup)
            .OfType<MovingHeadFixture>()
            .ForEach(mh => mh.EnableDelayedPosition = newProgram);
        }
    }
}


