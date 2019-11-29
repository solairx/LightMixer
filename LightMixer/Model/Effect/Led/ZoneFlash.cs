using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightMixer.Model.Fixture;

namespace LightMixer.Model
{
    public class ZoneFlashEffect : EffectBase
    {
        private int nextGroup = 0;
        public ZoneFlashEffect(BeatDetector.BeatDetector detector, Fixture.FixtureCollection currentValue, ObservableCollection<Fixture.FixtureGroup> vfixtureGroup, string vSchema)
            : base(detector, currentValue,vfixtureGroup,vSchema) { }

        public override void DmxFrameCall(DmxChaser.LedType ledInstance, IEnumerable<BeatDetector.VdjEvent> values)
        {
            var currentGroups = this.fixtureGroup.Where(o => o.Schema == Schema);
            if (currentGroups.Count() == 0) return;
            if (currentGroups.Count() - 1 < nextGroup)
                nextGroup = 0;
            var group = currentGroups.ElementAt(nextGroup);


            foreach (FixtureGroup groupall in currentGroups)
            {
                foreach (FixtureBase fixtureToreset in groupall.FixtureInGroup)
                {
                    ((RgbFixture)fixtureToreset).RedValue = this.SetValueFlash(0, ledInstance);
                    ((RgbFixture)fixtureToreset).GreenValue = this.SetValueFlash(0, ledInstance);
                    ((RgbFixture)fixtureToreset).BlueValue = this.SetValueFlash(0, ledInstance);
                }
            }

            if (isBeat)
            {
                foreach (FixtureBase fixture in group.FixtureInGroup)
                {

                    ((RgbFixture)fixture).RedValue = this.SetValueFlash(this._sharedEffectModel.Red, ledInstance);
                    ((RgbFixture)fixture).GreenValue = this.SetValueFlash(this._sharedEffectModel.Green, ledInstance);
                    ((RgbFixture)fixture).BlueValue = this.SetValueFlash(this._sharedEffectModel.Blue, ledInstance);
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
