using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using UIFrameWork;

namespace LightMixer.ViewModel
{
    public class ManualDmxControlViewModel : BaseViewModel  
    {
        private int mSelectedChannel = 0;
        private byte mSelectedValue = 0;

        public ManualDmxControlViewModel()
        {

        }

        public DmxModel DmxModel
        {
            get
            {
                return ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<DmxModel >();
            }
        }

   

        public int SelectedChannel
        {
            get
            {
                return mSelectedChannel;
            }
            set
            {
                mSelectedChannel = value;
                ObservableCollection<DmxChannelStatus> ChannelList = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<DmxModel>().DmxChannelStatus;

                 mSelectedValue = (byte)ChannelList.FirstOrDefault(
                        o => o.DmxChannelNumber == this.SelectedChannel
                        )
                    .DmxChannelValue;
                this.OnPropertyChanged(o => this.SelectedChannel);
                this.OnPropertyChanged(o => this.SelectedValue);
                ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<LightService.DmxServiceClient>().SetDmxChannel(this.SelectedChannel, (byte)this.SelectedValue);

            }
        }

        public byte SelectedValue
        {
            get
            {
                return mSelectedValue;
            }
            set
            {
                mSelectedValue = value;
                //client.SetDmxChannel(SelectedChannel, mSelectedValue);
                ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<DmxModel>().DmxChannelStatus
                    .Where(
                        o => o.DmxChannelNumber == this.SelectedChannel
                        )
                    .FirstOrDefault().DmxChannelValue = value;
                
            }
        }
    
    }
}
