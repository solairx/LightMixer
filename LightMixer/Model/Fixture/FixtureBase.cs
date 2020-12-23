using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightMixer.Model.Fixture
{

    public  abstract class FixtureBase 
    {
        public FixtureBase(int dmxAddress)
        {
            this.StartDmxAddress = dmxAddress;
        }

        public int StartDmxAddress
        {
            get;
            set;
        }
        
        public abstract int DmxLenght { get; }

        public abstract byte?[] Render();
    }
}
