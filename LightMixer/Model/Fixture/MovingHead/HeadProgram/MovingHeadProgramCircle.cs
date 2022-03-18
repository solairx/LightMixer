using System.Collections.Generic;
using System.Linq;
using static LightMixer.Model.Fixture.MovingHeadFixture;

namespace LightMixer.Model.Fixture
{
    public class MovingHeadProgramCircle : MovingHeadProgramPOVOnOff
    {
        public MovingHeadProgramCircle(bool isSlave, List<PointOfInterest> pointOfInterests) : base(isSlave, pointOfInterests.First(o => o.Location == PointOfInterestLocation.Circle))
        {
        }
        public override Program LegacyProgram => Program.Circle;
    }
}
