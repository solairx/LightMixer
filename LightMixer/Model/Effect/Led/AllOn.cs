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
        public FlashAllEffect(BeatDetector.BeatDetector detector, Fixture.FixtureCollection currentValue, ObservableCollection<Fixture.FixtureGroup> vfixtureGroup, string vSchema)
            : base(detector, currentValue,vfixtureGroup,vSchema) { }

        public override void DmxFrameCall(DmxChaser.LedType ledInstance)
        {
            isBeat = false;
            var workingGroup = this.fixtureGroup.Where(o => o.Schema == Schema);
            foreach (FixtureGroup group in workingGroup)
            {
                foreach (FixtureBase fixture in group.FixtureInGroup)
                {
                    if (fixture is RgbFixture)
                    {
                        ((RgbFixture)fixture).RedValue = this.SetValue(this._sharedEffectModel.Red, ledInstance);
                        ((RgbFixture)fixture).GreenValue = this.SetValue(this._sharedEffectModel.Green, ledInstance);
                        ((RgbFixture)fixture).BlueValue = this.SetValue(this._sharedEffectModel.Blue, ledInstance);
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
