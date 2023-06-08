using LightMixer.Model;
using Unity;
using System.Collections.Generic;
using System.Linq;


namespace LightMixer.View
{
    internal class TimeLineRenderedChaserViewModel : TimeLineViewModel
    {
        private SceneRenderedService sceneRendererService;
        public TimeLineRenderedChaserViewModel()
        {
            sceneRendererService = LightMixerBootStrap.UnityContainer.Resolve<SceneRenderedService>();
        }

        protected override void SongChanged()
        {
            var x = sceneRendererService.GetEffectBasedOnPosition();
            var newTimeLine = new List<ItemLineItemViewModel>();
            foreach (var chaserItem in x)
            {
                var item = new ItemLineItemViewModel { Position = chaserItem.Key, Name = chaserItem.Value.DisplayName, Color = chaserItem.Value.Color };
                newTimeLine.Add(item);
            }
            Items = newTimeLine;
            this.MaxPosition = Chaser.CurrentVdjSong.SongLenght;
            if (MaxPosition == 0)
                MaxPosition = x.Last().Key * 1.25;
        }
    }
}