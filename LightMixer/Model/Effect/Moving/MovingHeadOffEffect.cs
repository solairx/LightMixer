using System;
using System.Collections.Generic;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class MovingHeadOffEffect : RgbEffectBase
    {
        public MovingHeadOffEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter) { }

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
