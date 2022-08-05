using BeatDetector;
using LightMixer.Model.Fixture;
using System.Collections.Generic;
using System.Linq;

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
            if (workingEvent?.IsPoiPlausible == true && currentPoi.ID == 0)
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
                else if (nextPoi != null && GetSecondBeforeNextPOI(workingEvent, nextPoi) < 20)
                {
                    Before20SecBeatKickIn(dmxChaser, workingEvent, nextPoi);
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
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            Model.MaxSpeed = 1;
            dmxChaser.mBpmDetector.BeatRepeat = 1;
            SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.Med);
            dmxChaser.mBpmDetector.BeatRepeat = 1;

            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());

            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());

            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
        }

        private void Beat(DmxChaser dmxChaser)
        {
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            dmxChaser.mBpmDetector.BeatRepeat = 1;
            Model.MaxSpeed = 0.75;

            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            this.SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.High);

            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
        }

        private void BeatJustKickIn(DmxChaser dmxChaser)
        {
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            dmxChaser.mBpmDetector.BeatRepeat = 1;
            Model.MaxSpeed = 0.25;

            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            this.SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.Low);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
        }

        private void Chorus(DmxChaser dmxChaser)
        {
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, true);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            this.SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.off);
            Model.MaxSpeed = 1;
            dmxChaser.mBpmDetector.BeatRepeat = 1;
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<ZoneFlashEffect>().First());

            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First());

            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Circle);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, true);
            //this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, true);
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First());
        }

        private void Before20SecBeatKickIn(DmxChaser dmxChaser, VdjEvent workingEvent, VDJPoi nextPoi)
        {
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, true);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            this.SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.Low);

            Model.MaxSpeed = 1;
            dmxChaser.mBpmDetector.BeatRepeat = 1;

            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First());
            this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<AllOnEffect>().First());
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Circle);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, true);
        }

        private void BeforeBeatKickIn(DmxChaser dmxChaser, VdjEvent workingEvent, VDJPoi nextPoi)
        {
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            this.SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            this.SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            Model.MaxSpeed = 1;
            this.SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.High);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.DiscoBall);
            this.SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.DJ);
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 5)
            {
                this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
                this.SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
                this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<FlashAllEffect>().First());
            }
            else
            {
                this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneFlashEffect>().First());
                this.SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<ZoneFlashEffect>().First());
            }
            if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 3)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 10;
            }
            else if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 5)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 4;
            }
            else if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 6)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 2;
            }
            else if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 10)
            {
                dmxChaser.mBpmDetector.BeatRepeat = 2;
            }
            else
            {
                dmxChaser.mBpmDetector.BeatRepeat = 1;
            }
        }

        private void Reset(DmxChaser dmxChaser)
        {
        }

        private static double GetSecondBeforeNextPOI(VdjEvent workingEvent, VDJPoi nextPoi)
        {
            return (nextPoi.Position - workingEvent.Position) / workingEvent.BpmAsDouble / 1000;
        }

        private static double GetSecondInCurrentPoi(VdjEvent workingEvent)
        {
            return (workingEvent.Position - workingEvent.GetCurrentPoi.Position) / workingEvent.BpmAsDouble / 1000;
        }
    }
}