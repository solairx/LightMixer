﻿using LightMixer.Model.Fixture;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public class ZoneRotateEffect : EffectBase
    {
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
                foreach (var fixtureToreset in groupall.FixtureInGroup.OfType<RgbFixture>())
                {
                    fixtureToreset.RedValue = this.SetValueFlash(0);
                    fixtureToreset.GreenValue = this.SetValueFlash(0);
                    fixtureToreset.BlueValue = this.SetValueFlash(0);
                    if (!fixtureToreset.SupportRGB)
                    {
                        fixtureToreset.WhiteValue = 10;
                    }
                    else
                    {
                        fixtureToreset.WhiteValue = this.SetValueFlash(0);
                    }
                }
            }

            foreach (var fixture in group.FixtureInGroup.OfType<RgbFixture>())
            {
                fixture.RedValue = this.SetValue(this._sharedEffectModel.Red);
                fixture.GreenValue = this.SetValue(this._sharedEffectModel.Green);
                fixture.BlueValue = this.SetValue(this._sharedEffectModel.Blue);
                if (!fixture.SupportRGB)
                {
                    fixture.WhiteValue = 10;
                }
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