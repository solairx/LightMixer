namespace LightMixer.Model.Fixture
{
    public abstract class FixtureBase
    {
        public virtual void Init()
        { }

        private bool isInit = false;
        private EffectBase currentEffect1;

        public bool IsMaster
        {
            get
            {
                return OwnerGroup?.MasterFixture == this;
            }
        }

        public void InternalInit()
        {
            if (!isInit)
                Init();
        }

        public FixtureGroup OwnerGroup { get; set; }

        public EffectBase currentEffect { get => currentEffect1; set => currentEffect1 = value; }

        protected FixtureBase()
        { }

        public FixtureBase(int dmxAddress)
        {
            this.StartDmxAddress = dmxAddress;
        }

        public virtual bool IsRenderOnDmx => true;
        public virtual bool SupportAggresiveUpdate => true;

        public virtual WledServer HttpMulticastRenderer => null;

        public int StartDmxAddress
        {
            get;
            set;
        }

        public abstract int DmxLenght { get; }

        public abstract byte?[] Render();
    }
}