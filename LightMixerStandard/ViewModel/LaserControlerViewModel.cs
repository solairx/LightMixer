﻿using UIFrameWork;
using Unity;

namespace LightMixer.ViewModel
{
    public class LaserControlerViewModel : BaseViewModel
    {
        private LaserDisplay.LaserControler laser;
        private BeatDetector.BeatDetector mBpmDetector;

        public LaserControlerViewModel()
        {
           // laser = new LaserDisplay.LaserControler(-1);

            //laser.Start();
            mBpmDetector = LightMixerBootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
            // mBpmDetector.BeatEvent += new BeatDetector.BeatDetector.BeatHandler(mBpmDetector_BeatEvent);
            // mBpmDetector.BpmEvent += new BeatDetector.BeatDetector.BpmHandler(mBpmDetector_BpmEvent);
        }

        private void mBpmDetector_BpmEvent(double Beat, object caller)
        {
            //laser.OnMusic(100, Beat);
        }

        private void mBpmDetector_BeatEvent(bool Beat, object caller)
        {
            //laser.OnMusic(100, Beat);
        }

        public VisualControler.ServiceExchangeSingleton LaserConfig
        {
            get
            {
                return VisualControler.ServiceExchangeSingleton.Instance;
            }
            set
            {
                VisualControler.ServiceExchangeSingleton.Instance = value;
                AsyncOnPropertyChange(o => this.LaserConfig);
            }
        }
    }
}