using System.Collections.Generic;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramBalancing1 : MovingHeadProgram
    {
        public MovingHeadProgramBalancing1(FixtureBase owner, List<PointOfInterest> pointOfInterests) : base(owner)
        {

            PanListDesign = PanListDesignSlave = new ushort[] { 54000, 24000, 54000 };
            TiltListDesign = TiltListDesignSlave = new ushort[] { 500, 65535, 500, 65535, 500, 65535, 500 };
            MaxDimmerDesign = MaxDimmerDesignSlave = new ushort[] { 100 };
            InitialSize = 255;
            Setup();
        }
        public override Program LegacyProgram => Program.Balancing1;
    }
}
