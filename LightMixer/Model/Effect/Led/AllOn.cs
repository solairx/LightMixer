using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class AllOnEffect : RgbEffectBase
    {
        public AllOnEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter) { }

        public override void RenderEffect( IEnumerable<BeatDetector.VdjEvent> values)
        {
            isBeat = false;
            foreach (var fixture in fixtureInGroup)
            {
                fixture.RedValue = this.SetValue(this._sharedEffectModel.Red);
                fixture.GreenValue = this.SetValue(this._sharedEffectModel.Green);
                fixture.BlueValue = this.SetValue(this._sharedEffectModel.Blue);
                fixture.WhiteValue = 0;
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
