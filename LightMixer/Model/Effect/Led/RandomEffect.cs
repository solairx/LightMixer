using System.Linq;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;
using System;

namespace LightMixer.Model
{
    public class RandomEffect : EffectBase
    {

        int skip = 0;
        public RandomEffect(BeatDetector.BeatDetector detector, Fixture.FixtureCollection currentValue, ObservableCollection<Fixture.FixtureGroup> vfixtureGroup, string vSchema)
            : base(detector, currentValue, vfixtureGroup, vSchema) { }

        public override void DmxFrameCall(DmxChaser.LedType ledInstance)
        {
            if (skip > 0)
            {
                skip--;
                return;
            }
            skip = Convert.ToInt32( _sharedEffectModel.MaxSpeed);
            var rnd = new Random();
            var workingGroup = this.fixtureGroup.Where(o => o.Schema == Schema);
            foreach (FixtureGroup group in workingGroup)
            {
                foreach (FixtureBase fixture in group.FixtureInGroup)
                {
                    if (fixture is RgbFixture)
                    {
                        if (isBeat)
                        {
                            isBeat = false;
                            ((RgbFixture)fixture).RedValue = this.SetValueFlash(255, ledInstance);
                            ((RgbFixture)fixture).GreenValue = this.SetValueFlash(255, ledInstance);
                            ((RgbFixture)fixture).BlueValue = this.SetValueFlash(255, ledInstance);
                        }
                        else
                        {
                            ((RgbFixture)fixture).RedValue = this.SetValue((byte)(rnd.Next(255)/10), ledInstance);
                            ((RgbFixture)fixture).GreenValue = this.SetValue((byte)(rnd.Next(255) / 10), ledInstance);
                            ((RgbFixture)fixture).BlueValue = this.SetValue((byte)(rnd.Next(255) / 10), ledInstance);
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
