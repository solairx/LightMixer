using LightMixer.Model.Fixture;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public class MovingHeadDisco : EffectBase
    {

        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {


            foreach (FixtureBase fixture in Owner.FixtureGroups.SelectMany(o => o.FixtureInGroup))
            {
                if (fixture is MovingHeadFixture)
                {
                    ((MovingHeadFixture)fixture).ColorMode = MovingHeadFixture.ColorMacro.Auto;
                    ((MovingHeadFixture)fixture).GoboPaturn = _sharedEffectModel.CurrentMovingHeadGobo;
                    ((MovingHeadFixture)fixture).Pan = 500;
                    ((MovingHeadFixture)fixture).ProgramMode = MovingHeadFixture.Program.CodeDisable;

                    ((MovingHeadFixture)fixture).Speed = this.GetMaxedByte(this._sharedEffectModel.MaxSpeed * this.bpm);
                    ((MovingHeadFixture)fixture).RedValue = this.SetValueMovingHead(this._sharedEffectModel.Red);
                    ((MovingHeadFixture)fixture).GreenValue = this.SetValueMovingHead(this._sharedEffectModel.Green);
                    ((MovingHeadFixture)fixture).BlueValue = this.SetValueMovingHead(this._sharedEffectModel.Blue);


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
