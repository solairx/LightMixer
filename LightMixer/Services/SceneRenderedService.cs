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
        private DmxEffectSelector DmxEffectSelector = new DmxEffectSelector();
        private ConcurrentDictionary<int, VdjEvent> LastVdjEvent = new ConcurrentDictionary<int, VdjEvent>();
        private ActiveDeckSelector ActiveDeckSelector;
        private int FrameRate = 25; //25 ms
        private int UpdateUiRate = 4; // update every 4*25 ms (100ms )

        public SceneRenderedService(SceneService sceneService, DmxChaser legacyChaser, VComWrapper dmxWrapper, VirtualDjServer vdjServer)
        {
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
                vdjServer.VirtualDjServerEvent += VirtualDjServer_VirtualDjServerEvent;
        }

        private void Run()
        {
            while (isRunning)
            {
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
                    byte?[] ledArray = render(allfixture);
                    if (legacyChaser.LedEffectCollection.Count != 0)
                        if (legacyChaser.LedEffectCollection[0]._sharedEffectModel.AutoChangeColorOnBeat)
                        {
                            legacyChaser.LedEffectCollection[0]._sharedEffectModel.RotateColor();
                        }

                    for (int position = 0; position < ledArray.Length; position++)
                    {
                        if (dmxWrapper.Buffer[position] != ledArray[position])
                        {
                            dmxWrapper.Buffer = ConvertByteArray(ledArray);
                            break;
                        }
                    }
                }
                catch (Exception vexp)
                {
                    Debug.WriteLine(vexp.ToString());
                }
                Thread.Sleep(FrameRate);
            }
        }

        private byte?[] render(IEnumerable<FixtureBase> fixtures)
        {
            byte?[] array = new byte?[512];

            foreach (FixtureBase fixture in fixtures)
            {

                byte?[] fixtureValue = fixture.Render();

                int x = 0;
                for (x = fixture.StartDmxAddress; x < fixture.StartDmxAddress + fixture.DmxLenght; x++)
                {
                    if (fixtureValue[x].HasValue)
                        array[x] = fixtureValue[x].Value;
                }
            }
            return array;
        }

        private void VirtualDjServer_VirtualDjServerEvent(VdjEvent vdjEvent)
        {
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
            sceneService.Scenes
            .First(o => o.Name == scene)
            .Zones.Where(z => z.Name == zone)
            .Single().FixtureTypes.OfType<T>()
            .First().CurrentEffect = newEffect;
        }
    }
}


