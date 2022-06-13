using LightMixer.Model.Fixture;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public abstract class RgbEffectBase : EffectBase
    {
        protected IEnumerable<RgbFixtureBase> fixtureInGroup;

        protected override void OnFixtureCollectionChanged()
        {
            var workingGroup = Owner.FixtureGroups;
            fixtureInGroup = Owner.FixtureGroups
                .SelectMany(o => o.FixtureInGroup)
                .OfType<RgbFixtureBase>();
        }
    }
}
