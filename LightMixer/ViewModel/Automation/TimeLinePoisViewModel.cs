using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace LightMixer.View
{
    internal class TimeLinePoisViewModel : TimeLineViewModel
    {
        protected override void SongChanged()
        {
            var pois = Chaser.POIs;
            var currentsong = Chaser.CurrentVdjSong;
            if (pois != null && currentsong != null)
            {
                this.MaxPosition = Chaser.CurrentVdjSong.SongLenght;

                var newTimeLine = new List<ItemLineItemViewModel>();
                newTimeLine.Add(new ItemLineItemViewModel { Position = 0, Name = "I", Color = new SolidColorBrush(Colors.Yellow) }); //intro
                foreach (var poi in pois)
                {
                    var item = new ItemLineItemViewModel { Position = poi.PosAsDouble, Name = poi.Name };
                    if (poi.IsBreak)
                    {
                        item.Color = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        item.Color = new SolidColorBrush(Colors.Blue);
                    }
                    newTimeLine.Add(item);
                }
                Items = newTimeLine;

                if (MaxPosition == 0)
                    MaxPosition = pois.Last().PosAsDouble * 1.25;

            }
        }
    }
}