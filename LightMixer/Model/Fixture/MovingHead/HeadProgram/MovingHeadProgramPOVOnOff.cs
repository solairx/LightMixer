using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramPOVOnOff : MovingHeadProgram
    {
        public MovingHeadProgramPOVOnOff(bool isSlave, PointOfInterest pov) : base(isSlave)
        {
            InitialSize = 255;
            PanListDesign = new ushort[] { pov.Pan2, pov.Pan, pov.Pan2 };
            TiltListDesign = new ushort[] { pov.Tilt2, pov.Tilt, pov.Tilt2 };
            MaxDimmerDesign = new ushort[] { 100, 0 };

            PanListDesignSlave = new ushort[] { pov.Pan2, pov.Pan, pov.Pan2 };
            TiltListDesignSlave = new ushort[] { pov.Tilt2, pov.Tilt, pov.Tilt2 };
            MaxDimmerDesignSlave = new ushort[] { 100, 0 };
            if (isSlave)
            {
                position = 128;
            }

            Setup();
        }

        public override Program LegacyProgram => Program.DJ;
    }
}
