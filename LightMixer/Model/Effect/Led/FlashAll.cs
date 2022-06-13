using LightMixer.Model.Fixture;
using System.Collections.Generic;

namespace LightMixer.Model
{
    public class FlashAllEffect : EffectBase
    {
        public override WledEffect CurrentWledEffect => WledEffect.FX_MODE_STATIC;

        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            var workingGroup = Owner.FixtureGroups;
            if (isBeat || isSimulatedBeat)
            {
                isBeat = false;

                foreach (FixtureGroup group in workingGroup)
                {
                    foreach (FixtureBase fixture in group.FixtureInGroup)
                    {
                        if (fixture is RgbFixture && (!isSimulatedBeat || fixture.SupportAggresiveUpdate))
                        {
                            ((RgbFixture)fixture).RedValue = this.SetValueFlash(this._sharedEffectModel.Red);
                            ((RgbFixture)fixture).GreenValue = this.SetValueFlash(this._sharedEffectModel.Green);
                            ((RgbFixture)fixture).BlueValue = this.SetValueFlash(this._sharedEffectModel.Blue);
                            ((RgbFixture)fixture).WhiteValue = 255;
                        }
                    }
                }
                isSimulatedBeat = false;

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
                            ((RgbFixture)fixture).WhiteValue = 0;
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

        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            var workingGroup = Owner.FixtureGroups;
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
                            ((RgbFixture)fixture).RedValue = this.SetValue((byte)(this._sharedEffectModel.Red / 10));
                            ((RgbFixture)fixture).GreenValue = this.SetValue((byte)(this._sharedEffectModel.Green / 10));
                            ((RgbFixture)fixture).BlueValue = this.SetValue((byte)(this._sharedEffectModel.Blue / 10));
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
