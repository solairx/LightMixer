using LightMixer.Model.Fixture;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public class ZoneFlashEffect : EffectBase
    {
        public override WledEffect CurrentWledEffect => WledEffect.FX_MODE_STATIC;
        private int nextGroup = 0;


        public override void RenderEffect(IEnumerable<BeatDetector.VdjEvent> values)
        {
            var currentGroups = Owner.FixtureGroups;
            if (currentGroups.Count() == 0) return;
            if (currentGroups.Count() - 1 < nextGroup)
                nextGroup = 0;
            var group = currentGroups.ElementAt(nextGroup);


            foreach (FixtureGroup groupall in currentGroups)
            {
                foreach (FixtureBase fixtureToreset in groupall.FixtureInGroup)
                {
                    ((RgbFixture)fixtureToreset).RedValue = this.SetValueFlash(0);
                    ((RgbFixture)fixtureToreset).GreenValue = this.SetValueFlash(0);
                    ((RgbFixture)fixtureToreset).BlueValue = this.SetValueFlash(0);
                    ((RgbFixture)fixtureToreset).WhiteValue = this.SetValueFlash(0);
                }
            }

            if (isBeat)
            {
                foreach (FixtureBase fixture in group.FixtureInGroup)
                {

                    ((RgbFixture)fixture).RedValue = this.SetValueFlash(this._sharedEffectModel.Red);
                    ((RgbFixture)fixture).GreenValue = this.SetValueFlash(this._sharedEffectModel.Green);
                    ((RgbFixture)fixture).BlueValue = this.SetValueFlash(this._sharedEffectModel.Blue);
                    ((RgbFixture)fixture).WhiteValue = 0;
                }
                nextGroup++;
                isBeat = false;
            }




            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "Zone Flash Effect";
            }

        }
    }
}
