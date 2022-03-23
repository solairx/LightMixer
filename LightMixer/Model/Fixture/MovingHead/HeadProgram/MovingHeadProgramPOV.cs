using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramPOV : MovingHeadProgram
    {
        public MovingHeadProgramPOV(FixtureBase owner, PointOfInterest pov) : base(owner)
        {
            InitialSize = 255;
            PanListDesign = PanListDesignSlave = new ushort[] { pov.Pan, pov.Pan2, pov.Pan };
            TiltListDesign = TiltListDesignSlave = new ushort[] { pov.Tilt, pov.Tilt2, pov.Tilt };
            MaxDimmerDesign = MaxDimmerDesignSlave = new ushort[] { 100 };
            Setup();
        }

        public override Program LegacyProgram => Program.DJ;
    }
}
