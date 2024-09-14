using BeatDetector;
using LightMixer.Model.Fixture;
using System.Drawing;


namespace LightMixer.Model
{
    public class Beat : AutomatedEffect
    {
        public static string ID = "1";
        public Beat() : base(ID) {
            Color = Color.Brown;
            DisplayName = "Beat Rotate, MH DJ Flash"; }
        public override void RunInternal(VdjEvent workingEvent)
        {
            
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            dmxChaser.mBpmDetector.BeatRepeat = 1;
            Model.MaxSpeed = 0.75;

            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.High);

            SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, MovingHeadFixture.Program.Balancing1);
            SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, MovingHeadFixture.Program.Balancing1);
            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
            //SceneRenderedService.SetCurrentLaserEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, "Empty");
            SceneRenderedService.SetCurrentLaserEffectMood(SceneService.indoorSceneName, SceneService.djboothZoneName, LightMixerStandard.Model.Fixture.Laser.LaserEffectMood.Low, false);
        }
    }
}