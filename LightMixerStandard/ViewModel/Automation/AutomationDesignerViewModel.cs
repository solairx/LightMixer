using LightMixer.Model;
using Unity;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UIFrameWork;

namespace LightMixer.View
{

    public class AutomationDesignerViewModel : BaseViewModel
    {
        private SceneService sceneService;
        private SceneRenderedService sceneRendererService;
        public DmxChaser Chaser { get; }
        public SharedEffectModel SharedEffectModel { get; }
        public BeatDetector.BeatDetector BeatDetector { get; }

        public AutomationDesignerViewModel()
        {
            sceneService = LightMixerBootStrap.UnityContainer.Resolve<SceneService>();
            sceneRendererService = LightMixerBootStrap.UnityContainer.Resolve<SceneRenderedService>();
            Chaser = LightMixerBootStrap.UnityContainer.Resolve<DmxChaser>();
            SharedEffectModel = LightMixerBootStrap.UnityContainer.Resolve<SharedEffectModel>();
            BeatDetector = LightMixerBootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
        }

        public IEnumerable<SceneViewModel> Scenes
        {
            get
            {
                return sceneService.Scenes.Select(s => new SceneViewModel(s));
            }
        }

        public ICommand ResetBeatCommand
        {
            get
            {
                return new DelegateCommand(() => { BeatDetector.BeatRepeat = 1; });
            }
        }
    }
}