using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LightMixer.Model.Fixture
{


    public class RgbFixture : RgbFixtureBase
    {
        public RgbFixture(int dmxAddress) : base(dmxAddress) 
        {

        }
        public RgbFixture()
        {

        }


        public override byte?[] Render()
        {
            byte?[] arr = new byte?[512];
            arr[StartDmxAddress] = RedValue;
            arr[StartDmxAddress +1] = GreenValue;
            arr[StartDmxAddress +2] = BlueValue;
            return arr;
        }

        public override int DmxLenght => 4;

    }
}
