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

        protected RgbFixtureBase() { }

        protected RgbFixtureBase(int dmxAddress) : base(dmxAddress)
        {
        }
    }
}
