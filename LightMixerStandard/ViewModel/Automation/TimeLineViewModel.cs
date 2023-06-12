using LightMixer.Model;
using Unity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows;
using UIFrameWork;

namespace LightMixer.View
{

    public class TimeLineViewModel : BaseViewModel
    {
        private bool isEditable;

        private double timeLineWidth;
        private List<ItemLineItemViewModel> items;
        private double positionTicker;
        public bool IsEditable { get => isEditable; set => isEditable = value; }
        public DmxChaser Chaser { get; }
        public List<ItemLineItemViewModel> Items { get => items; set => items = value; }

        public double PositionTicker
        {
            get => positionTicker;
            set
            {
                positionTicker = value;
                AsyncOnPropertyChange(nameof(positionTicker));
            }
        }
        public TimeLineViewModel()
        {
            PositionTicker = 0 ;
            Items = new List<ItemLineItemViewModel>();
            Chaser = LightMixerBootStrap.UnityContainer.Resolve<DmxChaser>();
            Chaser.PoisChanged += Chaser_CurrentSongChanged;
            Chaser.PosChanged+= ChasePosChanged;
            this.MaxPosition = 600;

        }

        private void ChasePosChanged(object sender, EventArgs e)
        {
            RunOnUIThread(() =>
            {
                var elapsed = Chaser.Elapsed;
                if (elapsed > 0 && this.MaxPosition >0)
                {
                    PositionTicker = ((Chaser.Elapsed / 1000) / this.MaxPosition) * timeLineWidth; 
                    
                }
                else
                {
                    PositionTicker = 0;
                }
                
            });
        }

        private void Chaser_CurrentSongChanged(object sender, EventArgs e)
        {
            SongChangedInternal();

        }

        protected void SongChangedInternal()
        {
            RunOnUIThread(() =>
            {
                this.Items = new List<ItemLineItemViewModel>();
                SongChanged();
                ResizePanel();
            });
        }

        protected virtual void SongChanged()
        {

        }



        protected void ResizePanel()
        {

            double last = 0;
            ItemLineItemViewModel lastVm = null; ;
            foreach (var item in Items)
            {
                if (lastVm != null)
                {
                    lastVm.DisplayLenght = item.Position - last;
                    last = item.Position;
                }

                lastVm = item;
            }
            if (items.Any())
            {
                items.LastOrDefault().DisplayLenght = MaxPosition - last;
            }

            foreach (var item in Items)
            {
                item.DisplayLenght = (item.DisplayLenght / MaxPosition * TimeLineWidth);
            }

            OnPropertyChanged(nameof(Items));
        }

        public void ResizeIndividualItem(ItemLineItemViewModel itemLineItemViewModel, double horizontalChange)
        {
            itemLineItemViewModel.Position += MaxPosition / timeLineWidth * horizontalChange;
            ResizePanel();
        }

        public double TimeLineWidth //600
        {
            get => timeLineWidth;
            set
            {
                timeLineWidth = value;
                ResizePanel();
            }
        }

        public double MaxPosition { get; set; } //300


    }
}