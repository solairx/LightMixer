using LightMixer.Model.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public abstract class RgbEffectBase : EffectBase
    {
        protected IEnumerable<RgbFixtureBase> fixtureInGroup;

        public RgbEffectBase(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
           : base(detector, currentValue, intensityGetter, intensityFlashGetter)
        {
            var workingGroup = CurrentValue.FixtureGroups;
            fixtureInGroup = CurrentValue.FixtureGroups
                .SelectMany(o => o.FixtureInGroup)
                .OfType<RgbFixtureBase>();
        }
    }
}
