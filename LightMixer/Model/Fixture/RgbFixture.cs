using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightMixer.Model.Fixture
{
    public class RgbFixture : FixtureBase
    {
        public RgbFixture(int dmxAddress) : base(dmxAddress) 
        {

        }

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

        public override byte?[] Render()
        {
            byte?[] arr = new byte?[512];
            arr[StartDmxAddress] = RedValue;
            arr[StartDmxAddress +1] = GreenValue;
            arr[StartDmxAddress +2] = BlueValue;
            return arr;
        }

    }
}
