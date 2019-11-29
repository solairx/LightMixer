using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class AllOffEffect : EffectBase
    {
        public AllOffEffect(BeatDetector.BeatDetector detector, Fixture.FixtureCollection currentValue, ObservableCollection<Fixture.FixtureGroup> vfixtureGroup, string vSchema)
            : base(detector, currentValue,vfixtureGroup,vSchema) { }
        
        public override void DmxFrameCall(DmxChaser.LedType ledInstance, IEnumerable<BeatDetector.VdjEvent> values)
        {
            
                var workingGroup = this.fixtureGroup.Where(o => o.Schema == Schema);
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
