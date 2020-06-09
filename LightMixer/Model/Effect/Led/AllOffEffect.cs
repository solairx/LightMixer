using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;
using System;

namespace LightMixer.Model
{
    public class AllOffEffect : EffectBase
    {
        public AllOffEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter) { }
        
        public override void DmxFrameCall(IEnumerable<BeatDetector.VdjEvent> values)
        {
            
                var workingGroup = CurrentValue.FixtureGroups;
                foreach (FixtureGroup group in workingGroup)
                {
                    foreach (FixtureBase fixture in group.FixtureInGroup)
                    {
                        if (fixture is RgbFixture)
                        {
                            ((RgbFixture)fixture).RedValue = 0;
                            ((RgbFixture)fixture).GreenValue = 0;
                            ((RgbFixture)fixture).BlueValue = 0;
                        }
                    }
                }
            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "AllOff";
            }
           
        }
    }
}
