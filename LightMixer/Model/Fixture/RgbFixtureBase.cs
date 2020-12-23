namespace LightMixer.Model.Fixture
{
    public abstract class RgbFixtureBase : FixtureBase
    {
        public byte RedValue
        {
            get;
            set;
        }

        public byte GreenValue
        {
            get;
            set;
        }

        public byte BlueValue
        {
            get;
            set;
        }

        protected RgbFixtureBase(int dmxAddress) : base(dmxAddress)
        {
        }
    }
}
