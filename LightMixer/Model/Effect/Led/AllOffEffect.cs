using System.Collections.Generic;
using LightMixer.Model.Fixture;
using System;

namespace LightMixer.Model
{
    public class AllOffEffect : RgbEffectBase
    {
        public AllOffEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter)
        {

        }

        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            foreach (var fixture in fixtureInGroup)
            {
                if (fixture is WledFixture && fixture.currentEffect != this)
                    break;
                fixture.RedValue = 0;
                fixture.GreenValue = 0;
                fixture.BlueValue = 0;
                fixture.WhiteValue = 0;
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
