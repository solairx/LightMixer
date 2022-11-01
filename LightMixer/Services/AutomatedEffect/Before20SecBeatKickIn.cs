using BeatDetector;
using LightMixer.Model.Fixture;
using System.Linq;

namespace LightMixer.Model
{
    public class Before20SecBeatKickIn : AutomatedEffect
    {       
        public Before20SecBeatKickIn() : base(AutomatedEffectEnum.Before20SecBeatKickIn) { DisplayName = "Flash all, Balancing"; }
        public override void RunInternal(VdjEvent workingEvent)
        {
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, true);
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.Low);

            Model.MaxSpeed = 1;
            dmxChaser.mBpmDetector.BeatRepeat = 1;

            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadAllOn>().First());
            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<AllOffEffect>().First());
            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<AllOnEffect>().First());
            SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.basementZoneName, Fixture.MovingHeadFixture.Program.Balancing1);
            SceneRenderedService.SetMovingHeadProgramEffect(SceneService.indoorSceneName, SceneService.djboothZoneName, Fixture.MovingHeadFixture.Program.Circle);
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, true);
        }
    }
}