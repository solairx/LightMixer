using LightMixer.Model;
using Microsoft.Practices.Unity;
using UIFrameWork;

namespace LightMixer.View
{
    internal class TimeLineViewModel : BaseViewModel
    {
        private SceneService sceneService;
        private SceneRenderedService sceneRendererService;
        public DmxChaser Chaser { get; }
        public SharedEffectModel SharedEffectModel { get; }
        public BeatDetector.BeatDetector BeatDetector { get; }

        public TimeLineViewModel()
        {
            sceneService = BootStrap.UnityContainer.Resolve<SceneService>();
            sceneRendererService = BootStrap.UnityContainer.Resolve<SceneRenderedService>();
            Chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
            SharedEffectModel = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            BeatDetector = BootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
        }
    }
}