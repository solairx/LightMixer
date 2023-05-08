using LightMixer.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using UIFrameWork;

namespace LightMixer.View
{

    internal class AutomationDesignerViewModel : BaseViewModel
    {
        private SceneService sceneService;
        private SceneRenderedService sceneRendererService;
        public DmxChaser Chaser { get; }
        public SharedEffectModel SharedEffectModel { get; }
        public BeatDetector.BeatDetector BeatDetector { get; }

        public AutomationDesignerViewModel()
        {
            sceneService = BootStrap.UnityContainer.Resolve<SceneService>();
            sceneRendererService = BootStrap.UnityContainer.Resolve<SceneRenderedService>();
            Chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
            SharedEffectModel = BootStrap.UnityContainer.Resolve<SharedEffectModel>();
            BeatDetector = BootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
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