using System.Collections.Generic;

namespace LightMixer.Model
{
    public class AllOffEffect : RgbEffectBase
    {
        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            foreach (var fixture in fixtureInGroup)
            {
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