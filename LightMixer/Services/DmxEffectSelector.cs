using System.Collections.Generic;
using System.Linq;
using BeatDetector;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class DmxEffectSelector
    {
        public SceneRenderedService SceneRenderedService { get; }
        private SharedEffectModel Model { get; set; }

        public DmxEffectSelector(SceneRenderedService sceneService)
        {
            SceneRenderedService = sceneService;
        }

        public void Select(DmxChaser dmxChaser, IEnumerable<VdjEvent> lastEvent)
        {
            Model = dmxChaser.LedEffectCollection[0]._sharedEffectModel;
                        
            if (!dmxChaser.AutoChaser)
                return;
            
            var workingEvent = lastEvent.FirstOrDefault();

            var currentPoi = workingEvent?.GetCurrentPoi;

            var nextPoi = workingEvent?.GetNextPoi;

            Reset(dmxChaser);
            if (workingEvent?.IsPoiPlausible == true && currentPoi.ID == 0 )
            {
                Intro(dmxChaser);
            }
            else if (workingEvent?.IsPoiPlausible != true || currentPoi.ID == 0)
            {
                InvalidTrackInfo(dmxChaser);

            }
            else if (currentPoi != null && workingEvent.IsPoiPlausible)
            {
                PoiIsValidSelectEffect(dmxChaser, workingEvent, currentPoi, nextPoi);
            }

        }

        
        private void PoiIsValidSelectEffect(DmxChaser dmxChaser, VdjEvent workingEvent, VDJPoi currentPoi, VDJPoi nextPoi)
        {
            if (currentPoi.IsBreak)
            {
                if (dmxChaser.UseFlashTransition && nextPoi != null && GetSecondBeforeNextPOI(workingEvent, nextPoi) < 10)
                {
                    BeforeBeatKickIn(dmxChaser, workingEvent, nextPoi);
                }
                else
                {
                    Chorus(dmxChaser);
                }
            }
            else if (currentPoi.IsEndBreak && GetSecondInCurrentPoi(workingEvent) < 10)
            {
                BeatJustKickIn(dmxChaser);
            }
            else if (currentPoi.IsEndBreak)
            {
                Beat(dmxChaser);
            }
        }

        private void InvalidTrackInfo(DmxChaser dmxChaser)
        {
            Intro(dmxChaser);
        }

        private void Intro(DmxChaser dmxChaser)
        {
            SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.basementZoneName, WledEffectCategory.Med);
            dmxChaser.mBpmDetector.BeatRepeat = 1;
            dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First();
            dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneRotateEffect>().First();
            dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();
        }

        private void Beat(DmxChaser dmxChaser)
        {
            Model.MaxSpeed = 0.75;
            dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First();
            dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneRotateEffect>().First();
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());

            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
        }

        private void BeatJustKickIn(DmxChaser dmxChaser)
        {
            Model.MaxSpeed = 0.25;
            dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First();
            dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneRotateEffect>().First();
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            //this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.DiscoBall);
            //this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Circle);
            this.SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.basementZoneName, WledEffectCategory.Low);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
        }

        private void Chorus(DmxChaser dmxChaser)
        {
            dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneFlashEffect>().First();
            dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First();
            dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First();
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Circle);

        }

        private void BeforeBeatKickIn(DmxChaser dmxChaser, VdjEvent workingEvent, VDJPoi nextPoi)
        {
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.DiscoBall);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.DJ);
            dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();
            if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 5)
            {
                dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<FlashAllEffect>().First();
                dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<FlashAllEffect>().First();
            }
            else
            {
                dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneFlashEffect>().First();
                dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneFlashEffect>().First();
            }
            if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 3)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 2;
            }

            else if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 5)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 3;
            }

            else if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 6)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 5;
            }

            else if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 10)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 10;
            }
            else
            {
                dmxChaser.mBpmDetector.BeatRepeat = 1;
            }
        }

        private void Reset(DmxChaser dmxChaser)
        {
            dmxChaser.mBpmDetector.BeatRepeat = 1;
            Model.MaxSpeed = 1;
            this.SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.basementZoneName, WledEffectCategory.High);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
        }


        private static double GetSecondBeforeNextPOI(VdjEvent workingEvent, VDJPoi nextPoi)
        {
            return (nextPoi.Position - workingEvent.Position) / workingEvent.BpmAsDouble / 1000;
        }

        private static double GetSecondInCurrentPoi(VdjEvent workingEvent)
        {
            return (workingEvent.Position - workingEvent.GetCurrentPoi.Position ) / workingEvent.BpmAsDouble / 1000;
        }

    }
}
