using BeatDetector;
using LightMixer.Model.Fixture;
using System.Linq;
using System.Drawing;


namespace LightMixer.Model
{
    public class BeforeBeatKickIn : AutomatedEffect
    {
        public static string ID = "0";
        public BeforeBeatKickIn() : base(ID) 
        {
            Color = Color.Pink;
            DisplayName = "Beat repeat"; 
        }

        public override void RunInternal(VdjEvent workingEvent)
        {
            var nextPoi = workingEvent?.GetNextPoi;
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            Model.MaxSpeed = 1;
            SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.High);
            SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.DiscoBall);
            SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.DJ);
            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            if (GetSecondBeforeNextPOI(workingEvent, nextPoi) < 5)
            {
                SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
                SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
                SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<FlashAllEffect>().First());
            }
            else
            {
                SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneFlashEffect>().First());
                SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<ZoneFlashEffect>().First());
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
            //SceneRenderedService.SetCurrentLaserEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, "Empty");
            SceneRenderedService.SetCurrentLaserEffectMood(SceneService.indoorSceneName, SceneService.djboothZoneName, LightMixerStandard.Model.Fixture.Laser.LaserEffectMood.Low, false);
        }
    }
}