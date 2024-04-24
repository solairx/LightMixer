using LightMixer.Model;
using LightMixerStandard.Model.Fixture.Laser;
using System.Collections.Generic;
using System.Linq;
using UIFrameWork;

namespace LightMixer.View
{
    public class ZoneViewModel : BaseViewModel
    {
        private readonly Zone zone;

        public ZoneViewModel(Zone zone)
        {
            this.zone = zone;
        }

        public string Name => zone.Name;

        public IEnumerable<RGBLedViewModel> RGBLed
        {
            get
            {
                return zone.FixtureTypes
                    .OfType<RGBLedFixtureCollection>()
                    .Where(o=> !o.ContainWled)
                    .Select(s => new RGBLedViewModel(s));
            }
        }

        public IEnumerable<FeaturedRgbDesigner> Wled
        {
            get
            {
                return zone.FixtureTypes
                    .OfType<RGBLedFixtureCollection>()
                    .Where(o => o.ContainWled)
                    .Select(s => new FeaturedRgbDesigner(s));
            }
        }

        public IEnumerable<MovingHeadViewModel> MovingHead
        {
            get
            {
                return zone.FixtureTypes
                    .OfType<MovingHeadFixtureCollection>()
                    .Select(s => new MovingHeadViewModel(s));
            }
        }

        public IEnumerable<LaserViewModel> Laser
        {
            get
            {
                return zone.FixtureTypes
                    .OfType<LaserFixtureCollection>()
                    .Select(s => new LaserViewModel(s));
            }
        }
    }
}
