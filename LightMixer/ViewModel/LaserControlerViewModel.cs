using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using UIFrameWork;

namespace LightMixer.ViewModel
{
    public class LaserControlerViewModel : BaseViewModel  
    {
        private LaserDisplay.LaserControler laser;
        private BeatDetector.BeatDetector mBpmDetector;

        public LaserControlerViewModel()
        {
            laser = new LaserDisplay.LaserControler(-1);

            laser.Start();
            mBpmDetector = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<BeatDetector.BeatDetector>();
            mBpmDetector.BeatEvent += new BeatDetector.BeatDetector.BeatHandler(mBpmDetector_BeatEvent);
            mBpmDetector.BpmEvent += new BeatDetector.BeatDetector.BpmHandler(mBpmDetector_BpmEvent);
            
        }

        void mBpmDetector_BpmEvent(double Beat, object caller)
        {
            laser.OnMusic(100, Beat);
        }

        void mBpmDetector_BeatEvent(bool Beat, object caller)
        {
            laser.OnMusic(100, Beat);
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
                this.OnPropertyChanged(o => this.LaserConfig);
            }
        }

   

    }
}
