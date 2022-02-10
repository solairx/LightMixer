using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightMixer.Model.Fixture
{

    public  abstract class FixtureBase 
    {
        

        protected FixtureBase() { }

        public FixtureBase(int dmxAddress)
        {
            this.StartDmxAddress = dmxAddress;
        }

        public virtual bool IsRenderOnDmx  => true ;
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
