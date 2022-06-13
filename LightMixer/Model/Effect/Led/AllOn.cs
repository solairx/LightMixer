using System.Collections.Generic;

namespace LightMixer.Model
{
    public class AllOnEffect : RgbEffectBase
    {

        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            isBeat = false;
            foreach (var fixture in fixtureInGroup)
            {
                fixture.RedValue = this.SetValue(this._sharedEffectModel.Red);
                fixture.GreenValue = this.SetValue(this._sharedEffectModel.Green);
                fixture.BlueValue = this.SetValue(this._sharedEffectModel.Blue);
                fixture.WhiteValue = 255;
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
