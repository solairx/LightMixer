using LightMixer.Model;
using System.Collections.Generic;
using System.Linq;
using UIFrameWork;

namespace LightMixer.View
{
    public class SceneViewModel : BaseViewModel
    {
        private readonly Scene scene;

        public string Name => scene.Name;


        public SceneViewModel(Scene scene)
        {
            this.scene = scene;
        }

        public IEnumerable<ZoneViewModel> Zones
        {
            get
            {
                return scene.Zones.Select(s => new ZoneViewModel(s));
            }
        }
    }
}
