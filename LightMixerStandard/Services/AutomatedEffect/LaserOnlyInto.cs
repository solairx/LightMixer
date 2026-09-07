using BeatDetector;
using LightMixer.Model.Fixture;
using System.Drawing;


namespace LightMixer.Model
{
    public class LaserOnlyInto : AutomatedEffect
    {
        public static string ID = "7";
        public LaserOnlyInto() : base(ID)
        {
            Color = Color.Blue;
            DisplayName = "Laser Only Intro";
        }
        public override void RunInternal(VdjEvent workingEvent, bool isChained = false)
        {
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.off);
            Model.MaxSpeed = 1;
            dmxChaser.mBpmDetector.BeatRepeat = 1;
            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First());

            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First());

            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadOffEffect>().First());
            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadOffEffect>().First());
            SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Disable);
            SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Disable);
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            if (!isChained)
                SceneRenderedService.SetCurrentLaserEffectMood(SceneService.indoorSceneName, SceneService.djboothZoneName, LightMixerStandard.Model.Fixture.Laser.LaserEffectMood.Intro, true);
        }
    }
}