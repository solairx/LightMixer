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
                return BootStrap.UnityContainer.Resolve<DmxModel >();
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
                AsyncOnPropertyChange(o => this.Break);
                BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().SetBreak(value);
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
                AsyncOnPropertyChange(o => this.MBB); 
                BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().SetMBB(value);
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
                AsyncOnPropertyChange(o => this.Mab);
                
                BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().SetMab(value);
            }
        }

   

    }
}
