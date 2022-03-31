using System.Collections.Generic;
using System.Linq;
using BeatDetector;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class DmxEffectSelector
    {
        public SceneRenderedService SceneRenderedService { get; }

        public DmxEffectSelector(SceneRenderedService sceneService)
        {
            SceneRenderedService = sceneService;
        }

        public void Select(DmxChaser dmxChaser, IEnumerable<VdjEvent> lastEvent)
        {
            if (!dmxChaser.AutoChaser)
                return;
            var workingEvent = lastEvent.FirstOrDefault();

            var currentPoi = workingEvent?.GetCurrentPoi;

            var nextPoi = workingEvent?.GetNextPoi;


            if (workingEvent?.IsPoiPlausible != true || currentPoi.ID == 0)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 1;
                dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First();
                dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneRotateEffect>().First();
                dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First();

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
                    //Going to end break
                    HighIntensityFlashTransition(dmxChaser, workingEvent, nextPoi);
                }
                else
                {
                    // Break
                    LowIntensityMovingHeadFocus(dmxChaser);
                }
            }

            else if (currentPoi.IsEndBreak)
            {
                LowIntensityRotateEffet(dmxChaser);
            }
        }

        private void LowIntensityRotateEffet(DmxChaser dmxChaser)
        {
            dmxChaser.mBpmDetector.BeatRepeat = 0.25;
            dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First();
            dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneRotateEffect>().First();
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            //this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.DiscoBall);
            //this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Circle);

            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
        }

        private void LowIntensityMovingHeadFocus(DmxChaser dmxChaser)
        {
            dmxChaser.mBpmDetector.BeatRepeat = 1;
            dmxChaser.CurrentBoothEffect = dmxChaser.BoothEffectCollection.OfType<ZoneFlashEffect>().First();
            dmxChaser.CurrentLedEffect = dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First();
            dmxChaser.CurrentMovingHeadEffect = dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First();
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, true);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, true);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Circle);

        }

        private void HighIntensityFlashTransition(DmxChaser dmxChaser, VdjEvent workingEvent, VDJPoi nextPoi)
        {
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
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

        private static double GetSecondBeforeNextPOI(VdjEvent workingEvent, VDJPoi nextPoi)
        {
            return (nextPoi.Position - workingEvent.Position) / workingEvent.BpmAsDouble / 1000;
        }

    }
}
