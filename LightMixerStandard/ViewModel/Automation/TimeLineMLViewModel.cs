﻿using BeatDetector;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace LightMixer.View
{
    public class TimeLineMLViewModel : TimeLineViewModel
    {
        
        public int ConfidenceLevel { get; set; }
        public MLSongModel MlModel { get; private set; }
        
        protected override void SongChanged()
        {
            if (MlModel != null)
            {
                MlModel.MLLoaded -= MlModel_MLLoaded;
            }
            MlModel = Chaser.CurrentVdjSong?.MLSongModel;
            var mlPois = MlModel.MLPois?
                .OrderByDescending(o => o.Confidence)
                .Skip(ConfidenceLevel)
                .FirstOrDefault();
            if (mlPois != null)
            {
                var currentsong = Chaser.CurrentVdjSong;
                if (mlPois != null && currentsong != null)
                {
                    this.MaxPosition = Chaser.CurrentVdjSong.SongLenght;

                    var newTimeLine = new List<ItemLineItemViewModel>();
                    newTimeLine.Add(new ItemLineItemViewModel { Position = 0, Name = "I", Color = Color.Yellow }); //intro
                    MlModel.MLLoaded -= MlModel_MLLoaded;
                    foreach (var poi in mlPois.Pois)
                    {
                        var item = new ItemLineItemViewModel { Position = poi.PosAsDouble, Name = mlPois.Name };
                        if (poi.IsBreak)
                        {
                            item.Color = Color.Red;
                        }
                        else
                        {
                            item.Name = "";
                            item.Color = Color.Black;
                        }
                        newTimeLine.Add(item);
                    }
                    Items = newTimeLine;

                    if (MaxPosition == 0)
                        MaxPosition = mlPois.Pois.Last().PosAsDouble * 1.25;

                }

            }
            else
            {
                if (MlModel != null)
                {
                    MlModel.MLLoaded += MlModel_MLLoaded;
                }
            }
        }

        private void MlModel_MLLoaded(object sender, System.EventArgs e)
        {
            MlModel.MLLoaded -= MlModel_MLLoaded;
            if (sender == MlModel)
            {
                SongChangedInternal();
            }
        }
    }
}