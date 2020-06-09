using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class AllOnEffect : EffectBase
    {
        public AllOnEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter) { }

        public override void DmxFrameCall( IEnumerable<BeatDetector.VdjEvent> values)
        {
            isBeat = false;
            var workingGroup = CurrentValue.FixtureGroups;
            foreach (FixtureGroup group in workingGroup)
            {
                foreach (FixtureBase fixture in group.FixtureInGroup)
                {
                    if (fixture is RgbFixture)
                    {
                        ((RgbFixture)fixture).RedValue = this.SetValue(this._sharedEffectModel.Red);
                        ((RgbFixture)fixture).GreenValue = this.SetValue(this._sharedEffectModel.Green);
                        ((RgbFixture)fixture).BlueValue = this.SetValue(this._sharedEffectModel.Blue);
                    }
                }
            }


            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "AllOn";
            }
           
        }
    }
}
