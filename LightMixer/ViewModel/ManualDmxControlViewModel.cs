using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Linq;
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
                return BootStrap.UnityContainer.Resolve<DmxModel>();
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
                ObservableCollection<DmxChannelStatus> ChannelList = BootStrap.UnityContainer.Resolve<DmxModel>().DmxChannelStatus;

                mSelectedValue = (byte)ChannelList.FirstOrDefault(
                       o => o.DmxChannelNumber == this.SelectedChannel
                       )
                   .DmxChannelValue;
                AsyncOnPropertyChange(o => this.SelectedChannel);
                AsyncOnPropertyChange(o => this.SelectedValue);
                BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().SetDmxChannel(this.SelectedChannel, (byte)this.SelectedValue);

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
                BootStrap.UnityContainer.Resolve<DmxModel>().DmxChannelStatus
                    .Where(
                        o => o.DmxChannelNumber == this.SelectedChannel
                        )
                    .FirstOrDefault().DmxChannelValue = value;

            }
        }

    }
}
