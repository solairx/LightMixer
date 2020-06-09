using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class ZoneRotateEffect : EffectBase
    {
        private int nextGroup = 0;
        public ZoneRotateEffect(BeatDetector.BeatDetector detector, FixtureCollection currentValue, Func<double> intensityGetter, Func<double> intensityFlashGetter)
            : base(detector, currentValue, intensityGetter, intensityFlashGetter) { }

        public override void DmxFrameCall(IEnumerable<BeatDetector.VdjEvent> values)
        {
            var currentGroups = CurrentValue.FixtureGroups;
            if (currentGroups.Count() == 0) return;
            if (currentGroups.Count()-1 < nextGroup)
                nextGroup = 0;
            var group = currentGroups.ElementAt(nextGroup);
            foreach (FixtureGroup groupall in currentGroups)
            {
                foreach (FixtureBase fixtureToreset in groupall.FixtureInGroup)
                {
                    ((RgbFixture)fixtureToreset).RedValue = this.SetValueFlash(0);
                    ((RgbFixture)fixtureToreset).GreenValue = this.SetValueFlash(0);
                    ((RgbFixture)fixtureToreset).BlueValue = this.SetValueFlash(0);
                }
            }
            
            foreach (FixtureBase fixture in group.FixtureInGroup)
            {
                    ((RgbFixture)fixture).RedValue = this.SetValue(this._sharedEffectModel.Red);
                    ((RgbFixture)fixture).GreenValue = this.SetValue(this._sharedEffectModel.Green);
                    ((RgbFixture)fixture).BlueValue = this.SetValue(this._sharedEffectModel.Blue);
            }
            if (isBeat)
                nextGroup++;
            isBeat = false;

            this.RaiseEvent();
        }

        public override string Name
        {
            get
            {
                return "Zone Rotate Effect";
            }
           
        }
    }
}
