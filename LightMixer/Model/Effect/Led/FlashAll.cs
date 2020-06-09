using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class FlashAllEffect : EffectBase
    {
        public FlashAllEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter) { }

        public override void DmxFrameCall(IEnumerable<BeatDetector.VdjEvent> values)
        {
            var workingGroup = CurrentValue.FixtureGroups;
            if (isBeat)
            {
                isBeat = false;

                foreach (FixtureGroup group in workingGroup)
                {
                    foreach (FixtureBase fixture in group.FixtureInGroup)
                    {
                        if (fixture is RgbFixture)
                        {
                            ((RgbFixture)fixture).RedValue = this.SetValueFlash(this._sharedEffectModel.Red);
                            ((RgbFixture)fixture).GreenValue = this.SetValueFlash(this._sharedEffectModel.Green);
                            ((RgbFixture)fixture).BlueValue = this.SetValueFlash(this._sharedEffectModel.Blue);
                        }
                    }
                }

            }
            else
            {
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
            }
            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "FlashAll";
            }
           
        }
    }

    public class StaticColorFlashEffect : EffectBase
    {
        public StaticColorFlashEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter) { }

        public override void DmxFrameCall(IEnumerable<BeatDetector.VdjEvent> values)
        {
            var workingGroup = CurrentValue.FixtureGroups;
            if (isBeat)
            {
                isBeat = false;

                foreach (FixtureGroup group in workingGroup)
                {
                    foreach (FixtureBase fixture in group.FixtureInGroup)
                    {
                        if (fixture is RgbFixture)
                        {
                            ((RgbFixture)fixture).RedValue = this.SetValueFlash(this._sharedEffectModel.Red);
                            ((RgbFixture)fixture).GreenValue = this.SetValueFlash(this._sharedEffectModel.Green);
                            ((RgbFixture)fixture).BlueValue = this.SetValueFlash(this._sharedEffectModel.Blue);
                        }
                    }
                }

            }
            else
            {
                foreach (FixtureGroup group in workingGroup)
                {
                    foreach (FixtureBase fixture in group.FixtureInGroup)
                    {
                        if (fixture is RgbFixture)
                        {
                            ((RgbFixture)fixture).RedValue = this.SetValue((byte)(this._sharedEffectModel.Red/10));
                            ((RgbFixture)fixture).GreenValue = this.SetValue((byte)(this._sharedEffectModel.Green/10));
                            ((RgbFixture)fixture).BlueValue = this.SetValue((byte)(this._sharedEffectModel.Blue/10));
                        }
                    }
                }
            }
            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "Static Color Flash";
            }

        }
    }


}
