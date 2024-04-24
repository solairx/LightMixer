namespace LightMixer.Model.Fixture
{
    public class RgbwMovingHeadMasterFixture : MovingHeadFixture
    {
        public RgbwMovingHeadMasterFixture(int dmxAddress, List<PointOfInterest> pointOfInterests, double groupPosition) : base(dmxAddress, pointOfInterests)
        {
            GroupPosition = groupPosition;
        }

        public override byte?[] Render()
        {
            var codeProgram = RenderProgram();
            var panByte = BitConverter.GetBytes(Pan);
            var tiltByte = BitConverter.GetBytes(Tilt);

            if (codeProgram == null || Dimmer > 1 || Dimmer < 0)
            {
                Dimmer = 1;
            }

            byte?[] arr = new byte?[512];
            arr[StartDmxAddress] = panByte[1];//x
            arr[StartDmxAddress + 1] = panByte[0]; //x
            arr[StartDmxAddress + 2] = tiltByte[1];//y
            arr[StartDmxAddress + 3] = tiltByte[0];//y
            arr[StartDmxAddress + 4] = 0; //speed
            arr[StartDmxAddress + 5] = 255; //dimmer
            arr[StartDmxAddress + 6] = 0;//strobe
            if (EnableAlternateColor && (GroupPosition == 0.25 || GroupPosition == 0.75))
            {
                arr[StartDmxAddress + 7] = Convert.ToByte(Red2Value * Dimmer);
                arr[StartDmxAddress + 8] = Convert.ToByte(Green2Value * Dimmer);
                arr[StartDmxAddress + 9] = Convert.ToByte(Blue2Value * Dimmer);
                arr[StartDmxAddress + 10] = Convert.ToByte(White2Value * Dimmer);
            }
            else
            {
                arr[StartDmxAddress + 7] = Convert.ToByte(RedValue * Dimmer);
                arr[StartDmxAddress + 8] = Convert.ToByte(GreenValue * Dimmer);
                arr[StartDmxAddress + 9] = Convert.ToByte(BlueValue * Dimmer);
                arr[StartDmxAddress + 10] = Convert.ToByte(WhiteValue * Dimmer);
            }
            return arr;
        }

        public override int DmxLenght => 11;
    }
}