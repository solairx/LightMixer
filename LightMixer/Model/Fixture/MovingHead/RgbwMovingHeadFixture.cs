using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace LightMixer.Model.Fixture
{
    public class RgbwMovingHeadFixture : MovingHeadFixture
    {

        public RgbwMovingHeadFixture(int dmxAddress, List<PointOfInterest> pointOfInterests) : base(dmxAddress, pointOfInterests)
        {

        }



        public override byte?[] Render()
        {


            if (this.currentEffect?.GetType() == typeof(MovingHeadOffEffect))
            {

                byte?[] arr = new byte?[512];
                arr[StartDmxAddress] = 22;
                arr[StartDmxAddress + 1] = 0;
                arr[StartDmxAddress + 2] = 13;
                arr[StartDmxAddress + 3] = 0;
                arr[StartDmxAddress + 4] = 128;
                arr[StartDmxAddress + 5] = 255; //dimmer
                arr[StartDmxAddress + 6] = 0;//strobe
                arr[StartDmxAddress + 7] = 255;
                arr[StartDmxAddress + 8] = 255;
                arr[StartDmxAddress + 9] = 255;
                arr[StartDmxAddress + 10] = 255;
                /* arr[StartDmxAddress + 7] = c.R; //R
                 arr[StartDmxAddress + 8] = c.G; //G
                 arr[StartDmxAddress + 9] = c.B; //B
                 arr[StartDmxAddress + 10] = c.A; //white*/
                arr[StartDmxAddress + 11] = 0;
                return arr;
            }

            else if (this.currentEffect?.GetType() == typeof(MovingHeadFlashAll) && RedValue + GreenValue + BlueValue > 100 && BootStrap.UnityContainer.Resolve<DmxChaser>().CurrentLedEffect?.GetType() != typeof(ZoneRotateEffect))
            {
                byte?[] arr = new byte?[512];
                arr[StartDmxAddress] = 22;
                arr[StartDmxAddress + 1] = 0;
                arr[StartDmxAddress + 2] = 13;
                arr[StartDmxAddress + 3] = 0;
                arr[StartDmxAddress + 4] = 128;
                arr[StartDmxAddress + 5] = 255; //dimmer
                arr[StartDmxAddress + 6] = 0;//strobe
                arr[StartDmxAddress + 7] = 255;
                arr[StartDmxAddress + 8] = 255;
                arr[StartDmxAddress + 9] = 255;
                arr[StartDmxAddress + 10] = 255;
                /* arr[StartDmxAddress + 7] = c.R; //R
                 arr[StartDmxAddress + 8] = c.G; //G
                 arr[StartDmxAddress + 9] = c.B; //B
                 arr[StartDmxAddress + 10] = c.A; //white*/
                arr[StartDmxAddress + 11] = 0;
                return arr;
            }
            else if (this.currentEffect?.GetType() == typeof(MovingHeadFlashAll) && RedValue + GreenValue + BlueValue < 100 && BootStrap.UnityContainer.Resolve<DmxChaser>().CurrentLedEffect?.GetType() != typeof(ZoneRotateEffect))
            {
                byte?[] arr = new byte?[512];
                arr[StartDmxAddress] = 22;
                arr[StartDmxAddress + 1] = 0;
                arr[StartDmxAddress + 2] = 13;
                arr[StartDmxAddress + 3] = 0;
                arr[StartDmxAddress + 4] = 128;
                arr[StartDmxAddress + 5] = 255; //dimmer
                arr[StartDmxAddress + 6] = 0;//strobe
                arr[StartDmxAddress + 7] = 0;
                arr[StartDmxAddress + 8] = 0;
                arr[StartDmxAddress + 9] = 0;
                arr[StartDmxAddress + 10] = 0;
                /* arr[StartDmxAddress + 7] = c.R; //R
                 arr[StartDmxAddress + 8] = c.G; //G
                 arr[StartDmxAddress + 9] = c.B; //B
                 arr[StartDmxAddress + 10] = c.A; //white*/
                arr[StartDmxAddress + 11] = 0;
                return arr;
            }
            else if (this.currentEffect?.GetType() == typeof(MovingHeadFlashAll))
            {
                byte?[] arr = new byte?[512];
                arr[StartDmxAddress] = 0;
                arr[StartDmxAddress + 1] = 0;
                arr[StartDmxAddress + 2] = 0;
                arr[StartDmxAddress + 3] = 0;
                arr[StartDmxAddress + 4] = 255; //speed
                arr[StartDmxAddress + 5] = 64; //dimmer
                arr[StartDmxAddress + 6] = 0;//strobe
                arr[StartDmxAddress + 7] = 255;
                arr[StartDmxAddress + 8] = 0;
                arr[StartDmxAddress + 9] = 0;
                arr[StartDmxAddress + 10] = 0; //white
                arr[StartDmxAddress + 11] = 0;
                return arr;
                //Color c = Color.FromArgb(RedValue, GreenValue, BlueValue);

            }
            else
            {
                byte?[] arr = new byte?[512];
                arr[StartDmxAddress] = 0; //x 
                arr[StartDmxAddress + 1] = 0;//x
                arr[StartDmxAddress + 2] = 0;//y
                arr[StartDmxAddress + 3] = 0;//y
                arr[StartDmxAddress + 4] = 255; //speed
                arr[StartDmxAddress + 5] = 255; //dimmer
                arr[StartDmxAddress + 6] = 0;//strobe
                arr[StartDmxAddress + 7] = RedValue;
                arr[StartDmxAddress + 8] = GreenValue;
                arr[StartDmxAddress + 9] = BlueValue;
                arr[StartDmxAddress + 10] = 0; //white
                arr[StartDmxAddress + 11] = 0;
                return arr;
                //Color c = Color.FromArgb(RedValue, GreenValue, BlueValue);


            }

        }

        public override int DmxLenght => 13;

    }
}
