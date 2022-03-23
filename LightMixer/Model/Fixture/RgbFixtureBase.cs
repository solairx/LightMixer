using System.Drawing;

namespace LightMixer.Model.Fixture
{
    public abstract class RgbFixtureBase : FixtureBase
    {
        private byte redValue;
        private byte blueValue;
        protected bool isDitry = false;
        private byte greenValue;
        private byte whiteValue;

        public byte RedValue
        {
            get => redValue;
            set
            {
                if (redValue != value)
                {
                    redValue = value;
                    isDitry = true;
                }
            }
        }

        public byte Red2Value => Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb() ^ 0xffffff).R;
        public byte Red3Value => Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb() ^ 0x777777).R;


        public byte GreenValue
        {
            get => greenValue;
            set 
            {
                if (greenValue != value)
                {
                    greenValue = value;
                    isDitry = true;
                }
            }
        }

        public byte Green2Value => Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb() ^ 0xffffff).G;
        public byte Green3Value => Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb() ^ 0x777777).G;

        public byte BlueValue
        {
            get => blueValue;
            set
            {
                if (blueValue != value)
                {
                    blueValue = value;
                    isDitry = true;
                }
            }
        }

        public byte Blue2Value => Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb() ^ 0xffffff).B;
        public byte Blue3Value => Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb() ^ 0x777777).B;

        public byte WhiteValue
        {
            get => whiteValue;
            set
            {
                if (whiteValue != value)
                {
                    whiteValue = value;
                    isDitry = true;
                }
            }
        }

        public byte White2Value => Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb() ^ 0xffffff).A;
        public byte White3Value => Color.FromArgb(Color.FromArgb(RedValue, GreenValue, BlueValue).ToArgb() ^ 0x777777).A;

        protected RgbFixtureBase() { }

        protected RgbFixtureBase(int dmxAddress) : base(dmxAddress)
        {
        }
    }
}
