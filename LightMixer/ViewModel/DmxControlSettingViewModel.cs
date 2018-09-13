using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using UIFrameWork;

namespace LightMixer.ViewModel
{
    public class DmxControlSettingViewModel : BaseViewModel  
    {
        private long mBreak = 27600;
        private long mMbb = 0;
        private long mMab = 200;

        public DmxControlSettingViewModel()
        {

        }

        public DmxModel DmxModel
        {
            get
            {
                return ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<DmxModel >();
            }
        }


        public long Break
        {
            get
            {
                return mBreak;
            }
            set
            {
                mBreak = value;
                this.OnPropertyChanged(o => this.Break);
                ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<LightService.DmxServiceClient>().SetBreak(value);
            }
        }

        public long MBB
        {
            get
            {
                return mMbb;
            }
            set
            {
                mMbb = value;
                this.OnPropertyChanged(o => this.MBB); 
                ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<LightService.DmxServiceClient>().SetMBB(value);
            }
        }
        public long Mab
        {
            get
            {
                return mMab;
            }
            set
            {
                this.mMab = value;
                this.OnPropertyChanged(o => this.Mab);
                
                ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<LightService.DmxServiceClient>().SetMab(value);
            }
        }

   

    }
}
