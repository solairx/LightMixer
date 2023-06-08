using LightMixer.Model;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using UIFrameWork;

namespace LightMixer.View
{

    internal class TimeLineViewModel : BaseViewModel
    {
        private bool isEditable;

        private double timeLineWidth;
        private List<ItemLineItemViewModel> items;
        private Thickness positionTicker;
        public bool IsEditable { get => isEditable; internal set => isEditable = value; }
        public DmxChaser Chaser { get; }
        public List<ItemLineItemViewModel> Items { get => items; set => items = value; }

        public Thickness PositionTicker
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
            PositionTicker = new Thickness(0, 0, 0, 0); ;
            Items = new List<ItemLineItemViewModel>();
            Chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
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
                    PositionTicker = new Thickness(((Chaser.Elapsed / 1000) / this.MaxPosition) * timeLineWidth, 0, 0, 0); 
                    
                }
                else
                {
                    PositionTicker = new Thickness(0, 0, 0, 0);
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

        internal void ResizeIndividualItem(ItemLineItemViewModel itemLineItemViewModel, DragDeltaEventArgs e)
        {
            itemLineItemViewModel.Position += MaxPosition / timeLineWidth * e.HorizontalChange;
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