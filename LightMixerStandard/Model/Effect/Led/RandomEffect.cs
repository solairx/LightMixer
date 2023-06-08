using LightMixer.Model.Fixture;
using System;
using System.Collections.Generic;

namespace LightMixer.Model
{
    public class RandomEffect : EffectBase
    {
        private int skip = 0;

        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            if (skip > 0)
            {
                skip--;
                return;
            }
            skip = Convert.ToInt32(_sharedEffectModel.MaxSpeed);
            var rnd = new Random();
            var workingGroup = Owner.FixtureGroups;
            foreach (FixtureGroup group in workingGroup)
            {
                foreach (FixtureBase fixture in group.FixtureInGroup)
                {
                    if (fixture is RgbFixture)
                    {
                        if (isBeat)
                        {
                            isBeat = false;
                            ((RgbFixture)fixture).RedValue = this.SetValueFlash(255);
                            ((RgbFixture)fixture).GreenValue = this.SetValueFlash(255);
                            ((RgbFixture)fixture).BlueValue = this.SetValueFlash(255);
                            ((RgbFixture)fixture).WhiteValue = 0;
                        }
                        else
                        {
                            ((RgbFixture)fixture).RedValue = this.SetValue((byte)(rnd.Next(255) / 10));
                            ((RgbFixture)fixture).GreenValue = this.SetValue((byte)(rnd.Next(255) / 10));
                            ((RgbFixture)fixture).BlueValue = this.SetValue((byte)(rnd.Next(255) / 10));
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
                return "Random  Flash Effect";
            }
        }
    }
}