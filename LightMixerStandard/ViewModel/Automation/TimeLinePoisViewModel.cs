using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace LightMixer.View
{
    public class TimeLinePoisViewModel : TimeLineViewModel
    {
        protected override void SongChanged()
        {
            var pois = Chaser.POIs;
            var currentsong = Chaser.CurrentVdjSong;
            if (pois != null && currentsong != null)
            {
                this.MaxPosition = Chaser.CurrentVdjSong.SongLenght;

                var newTimeLine = new List<ItemLineItemViewModel>();
                newTimeLine.Add(new ItemLineItemViewModel { Position = 0, Name = "I", Color = Color.Yellow }); //intro
                foreach (var poi in pois)
                {
                    var item = new ItemLineItemViewModel { Position = poi.PosAsDouble, Name = poi.Name };
                    if (poi.IsBreak)
                    {
                        item.Color = Color.Green;
                    }
                    else
                    {
                        item.Color = Color.Blue;
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