using System.Collections.Generic;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramDisable : MovingHeadProgram
    {
        public MovingHeadProgramDisable(FixtureBase owner, List<PointOfInterest> pointOfInterests) : base(owner)
        {
            PanListDesign = PanListDesignSlave = new ushort[] { 0, 0 };
            TiltListDesign = TiltListDesignSlave = new ushort[] { 0, 0 };
            MaxDimmerDesign = MaxDimmerDesignSlave = new ushort[] { 0, 0 };
            InitialSize = 10;
            Setup();
        }

        public override Program LegacyProgram => Program.CodeDisable;
    }
}