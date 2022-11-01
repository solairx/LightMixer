using BeatDetector;
using LightMixer.Model.Fixture;
using System.Linq;

namespace LightMixer.Model
{
    public class Intro : AutomatedEffect
    {
        public Intro() : base(AutomatedEffectEnum.Intro) { DisplayName = "Intro Alternate+Med+Rotate MH=Flash"; }
        public override void RunInternal(VdjEvent workingEvent)
        {
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.basementZoneName, false);
            SceneRenderedService.SetMovingHeadAlternateColor(SceneService.indoorSceneName, SceneService.djboothZoneName, false);
            SceneRenderedService.SetMovingHeadDelayedPosition(SceneService.indoorSceneName, SceneService.djboothZoneName, false);

            Model.MaxSpeed = 1;
            dmxChaser.mBpmDetector.BeatRepeat = 1;
            SceneRenderedService.SetWledFixtureFocus(SceneService.indoorSceneName, SceneService.djboothZoneName, WledEffectCategory.Med);
            dmxChaser.mBpmDetector.BeatRepeat = 1;

            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());

            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());

            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            SceneRenderedService.SetCurrentEffect<MovingHeadFixtureCollection>(SceneService.indoorSceneName, SceneService.djboothZoneName, dmxChaser.MovingHeadEffectCollection.OfType<MovingHeadFlashAll>().First());
            SceneRenderedService.SetCurrentEffect<RGBLedFixtureCollection>(SceneService.indoorSceneName, SceneService.basementZoneName, dmxChaser.LedEffectCollection.OfType<ZoneRotateEffect>().First());
        }
    }
}