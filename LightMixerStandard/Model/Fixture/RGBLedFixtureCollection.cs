using LightMixer.Model.Fixture;
using System.Linq;

namespace LightMixer.Model
{
    public class RGBLedFixtureCollection : FixtureCollection
    {
        public bool ContainWled
        {
            get => this.FixtureGroups?.SelectMany(o => o.FixtureInGroup)
                .OfType<WledFixture>()?.Any() == true;
        }
    }
}