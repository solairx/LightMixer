using System.Collections.Generic;
using System.Linq;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramDiscoBall : MovingHeadProgramPOV
    {
        public MovingHeadProgramDiscoBall(bool isSlave, List<PointOfInterest> pointOfInterests) : base(isSlave, pointOfInterests.First(o => o.Location == PointOfInterestLocation.DiscoBall))
        {
        }

        public override Program LegacyProgram => Program.DiscoBall;
    }
}
