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
                AsyncOnPropertyChange(o => this.mDmxChannelNumber);
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
                AsyncOnPropertyChange(o => this.DmxChannelValue);
                //   BootStrap.UnityContainer.Resolve<LightService.DmxServiceClient>().SetDmxChannel(mDmxChannelNumber,(byte)mDmxChannelValue );
            }
        }
    }
}