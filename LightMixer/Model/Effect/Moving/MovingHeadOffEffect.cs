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
        public MovingHeadOffEffect(BeatDetector.BeatDetector detector, Fixture.FixtureCollection currentValue, ObservableCollection<Fixture.FixtureGroup> vfixtureGroup)
            : base(detector, currentValue, vfixtureGroup, "default") { }
        
        public override void DmxFrameCall(DmxChaser.LedType ledInstance, IEnumerable<BeatDetector.VdjEvent> values)
        {

                foreach (FixtureBase fixture in CurrentValue.FixtureList)
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
