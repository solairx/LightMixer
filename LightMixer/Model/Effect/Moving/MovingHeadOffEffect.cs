using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class MovingHeadOffEffect : EffectBase
    {
        public MovingHeadOffEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter) { }

        public override void DmxFrameCall(IEnumerable<BeatDetector.VdjEvent> values)
        {

                foreach (FixtureBase fixture in CurrentValue.FixtureGroups.SelectMany(o=>o.FixtureInGroup))
                {
                    if (fixture is MovingHeadFixture)
                    {
                        ((MovingHeadFixture)fixture).RedValue = 0;
                        ((MovingHeadFixture)fixture).GreenValue = 0;
                        ((MovingHeadFixture)fixture).BlueValue = 0;
                    }
                }
            
            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "MovingAllOff";
            }
           
        }
    }
}
