using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace LightMixer
{
    public class DmxChannelStatus : UIFrameWork.BaseViewModel
    {
        private int mDmxChannelNumber = 0;
        private int mDmxChannelValue = 0;
        public int DmxChannelNumber
        {
            get
            {
                return mDmxChannelNumber;

            }
            set
            {
                mDmxChannelNumber = value;
                this.OnPropertyChanged(o => this.mDmxChannelNumber);
            }
        }

        public int DmxChannelValue
        {
            get
            {
                return mDmxChannelValue;

            }
            set
            {
                mDmxChannelValue = value;
                this.OnPropertyChanged(o => this.DmxChannelValue);
             //   ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<LightService.DmxServiceClient>().SetDmxChannel(mDmxChannelNumber,(byte)mDmxChannelValue );
            }
        }


    }
}
