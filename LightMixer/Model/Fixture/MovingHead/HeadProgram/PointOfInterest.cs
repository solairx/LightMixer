namespace LightMixer.Model.Fixture
{
    public class PointOfInterest
    {
        private ushort pan2;
        private ushort tilt2;

        public ushort Pan { get; set; }
        public ushort Tilt { get; set; }

        public ushort Pan2
        {
            get => pan2 != 0 ? pan2 : Pan; set
            {
                pan2 = value;
                if (value == 0)
                {
                    pan2 = 1;
                }
            }
        }
        public ushort Tilt2
        {
            get => tilt2 != 0 ? tilt2 : Tilt; set
            {
                tilt2 = value;
                if (tilt2 == 0)
                {
                    tilt2 = 1;
                }
                
            }
        }

        public PointOfInterestLocation Location { get; set; }
    }
}
