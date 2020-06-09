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

namespace LightMixer.Model
{
    public class SceneRenderedService
    {
        private readonly SceneService sceneService;
        private readonly BeatDetector.BeatDetector beatDetector;
        private readonly DmxChaser legacyChaser;
        private bool isRunning = true;
        private DateTime LastUpdateOnUI = DateTime.Now;
        private Thread runningThread;
        private DmxEffectSelector DmxEffectSelector = new DmxEffectSelector();
        private ConcurrentDictionary<int, VdjEvent> LastVdjEvent = new ConcurrentDictionary<int, VdjEvent>();
        private ActiveDeckSelector ActiveDeckSelector;

        public SceneRenderedService(SceneService sceneService, BeatDetector.BeatDetector beatDetector, DmxChaser legacyChaser)
        {
            ActiveDeckSelector = new ActiveDeckSelector();
            LastVdjEvent[1] = new VdjEvent();
            LastVdjEvent[2] = new VdjEvent();
            LastVdjEvent[3] = new VdjEvent();
            LastVdjEvent[4] = new VdjEvent();
            this.sceneService = sceneService;
            this.beatDetector = beatDetector;
            this.legacyChaser = legacyChaser;
            runningThread = new Thread(new ThreadStart(Run));
            runningThread.IsBackground = true;
            runningThread.Start();
            if (beatDetector.VirtualDjServer != null)
                beatDetector.VirtualDjServer.VirtualDjServerEvent += VirtualDjServer_VirtualDjServerEvent;
        }

        private void Run()
        {
            while (isRunning)
            {
                try
                {
                    var activeDeck = ActiveDeckSelector.Select(LastVdjEvent.Values);
                    if (DateTime.Now.Subtract(LastUpdateOnUI).TotalMilliseconds > 100)
                    {
                        legacyChaser.UpdateVDJUiElement(activeDeck);
                        LastUpdateOnUI = DateTime.Now;
                        if (legacyChaser.POIs?.Any() == false)
                        {
                            beatDetector.VirtualDjServer.vdjDataBase.CheckForRefresh();
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
                    BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().UpdateAllDmxValue(ConvertByteArray(ledArray));
                }
                catch (Exception vexp)
                {
                }
                Thread.Sleep(25);
            }
        }

        private byte?[] render(IEnumerable<FixtureBase> fixtures)
        {
            byte?[] array = new byte?[512];

            foreach (FixtureBase fixture in fixtures)
            {

                byte?[] fixtureValue = fixture.Render();

                int x = 0;
                for (x = 0; x < 512; x++)
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
            return sceneService.Scenes
                .First(o => o.Name == scene)
                .Zones.Where(z => z.Name == zone)
                .First().FixtureTypes.OfType<T>()
                .First().CurrentEffect;
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


