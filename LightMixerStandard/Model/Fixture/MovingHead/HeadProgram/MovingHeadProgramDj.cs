using System.Collections.Generic;
using System.Linq;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramDj : MovingHeadProgramPOV
    {
        public MovingHeadProgramDj(FixtureBase owner, List<PointOfInterest> pointOfInterests) : base(owner, pointOfInterests.First(o => o.Location == PointOfInterestLocation.DJ))
        {
        }

        public override Program LegacyProgram => Program.DJ;
    }
}