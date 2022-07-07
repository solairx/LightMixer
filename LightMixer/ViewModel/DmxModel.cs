using System.Collections.ObjectModel;
using UIFrameWork;

namespace LightMixer
{
    public class DmxModel : BaseViewModel
    {
        private ObservableCollection<DmxChannelStatus> mDmxChannelStatus = new ObservableCollection<DmxChannelStatus>();

        public DmxModel()
        {
            int x = 0;
            for (x = 0; x < 512; x++)
            {
                mDmxChannelStatus.Add(new DmxChannelStatus() { DmxChannelNumber = x, DmxChannelValue = 0 });
            }
        }

        public ObservableCollection<DmxChannelStatus> DmxChannelStatus
        {
            get
            {
                return mDmxChannelStatus;
            }
            set
            {
                mDmxChannelStatus = value;
            }
        }

        public int MinDmxValue
        {
            get
            {
                return 0;
            }
        }

        public int MaxDmxValue
        {
            get
            {
                return 255;
            }
        }
    }
}