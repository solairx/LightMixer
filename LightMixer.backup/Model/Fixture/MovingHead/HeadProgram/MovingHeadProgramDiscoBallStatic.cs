using System.Collections.Generic;
using System.Linq;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramDiscoBallStatic : MovingHeadProgramPOV
    {
        public MovingHeadProgramDiscoBallStatic(FixtureBase owner, List<PointOfInterest> pointOfInterests) : base(owner, pointOfInterests.First(o => o.Location == PointOfInterestLocation.DiscoBallStatic))
        {
        }

        public override Program LegacyProgram => Program.DiscoBallStatic;
    }
}