using LightMixer.Model.Fixture;
using System.Collections.Generic;

namespace LightMixer.Model
{
    public class MovingHeadOffEffect : RgbEffectBase
    {


        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            foreach (var fixture in fixtureInGroup)
            {
                if (fixture is MovingHeadFixture)
                {
                    fixture.RedValue = 0;
                    fixture.GreenValue = 0;
                    fixture.BlueValue = 0;
                    (fixture as MovingHeadFixture).ProgramMode = MovingHeadFixture.Program.CodeDisable;
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
