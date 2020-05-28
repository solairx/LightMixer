using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class MovingHeadDisco : EffectBase
    {
        public MovingHeadDisco(BeatDetector.BeatDetector detector, Fixture.FixtureCollection currentValue, ObservableCollection<Fixture.FixtureGroup> vfixtureGroup)
            : base(detector, currentValue,vfixtureGroup,"default") { }
        
        public override void DmxFrameCall(DmxChaser.LedType ledInstance, IEnumerable<BeatDetector.VdjEvent> values)
        {
            
            
                foreach ( FixtureBase fixture in CurrentValue.FixtureList)
                {
                    if (fixture is MovingHeadFixture)
                    {
                        ((MovingHeadFixture)fixture).ColorMode = MovingHeadFixture.ColorMacro.Auto;
                        ((MovingHeadFixture)fixture).GoboPaturn = _sharedEffectModel.CurrentMovingHeadGobo;
                        ((MovingHeadFixture)fixture).Pan = 500;
                        ((MovingHeadFixture)fixture).ProgramMode = MovingHeadFixture.Program.Disable;

                        ((MovingHeadFixture)fixture).Speed = this.GetMaxedByte(this._sharedEffectModel.MaxSpeed * this.bpm);
                        ((MovingHeadFixture)fixture).RedValue = this.SetValueMovingHead(this._sharedEffectModel.Red, ledInstance);
                        ((MovingHeadFixture)fixture).GreenValue = this.SetValueMovingHead(this._sharedEffectModel.Green, ledInstance);
                        ((MovingHeadFixture)fixture).BlueValue = this.SetValueMovingHead(this._sharedEffectModel.Blue, ledInstance);

                    
                    }
                }

                if (isBeat)
                {
                    isBeat = false;
                }

            
               
            
            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "Disco";
            }
           
        }
    }
}
