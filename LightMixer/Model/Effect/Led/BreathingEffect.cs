using LightMixer.Model.Fixture;
using System.Collections.Generic;

namespace LightMixer.Model
{
    public class BreathingEffect : EffectBase
    {
        double next = 1;
        bool direction = true;


        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            if (direction)
                next += 1 * _sharedEffectModel.MaxSpeed;
            else
                next -= 1 * _sharedEffectModel.MaxSpeed;
            isBeat = false;


            if (next > 99)
            {
                next = 99;
                direction = false;
            }
            if (next < 10)
            {
                next = 10;
                direction = true;
            }

            var workingGroup = Owner.FixtureGroups;
            foreach (FixtureGroup group in workingGroup)
            {
                foreach (FixtureBase fixture in group.FixtureInGroup)
                {
                    if (fixture is RgbFixture)
                    {

                        ((RgbFixture)fixture).RedValue = this.SetValue((byte)(this._sharedEffectModel.Red * next / 100d));
                        ((RgbFixture)fixture).GreenValue = this.SetValue((byte)(this._sharedEffectModel.Green * next / 100d));
                        ((RgbFixture)fixture).BlueValue = this.SetValue((byte)(this._sharedEffectModel.Blue * next / 100d));
                        ((RgbFixture)fixture).WhiteValue = 0;
                    }
                }
            }


            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "Breath";
            }

        }
    }
}
