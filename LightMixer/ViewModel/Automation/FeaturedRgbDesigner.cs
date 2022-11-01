using LightMixer.Model;
using LightMixer.Model.Fixture;
using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UIFrameWork;

namespace LightMixer.View
{

    public abstract class FeaturedRgbDesignerBase : BaseViewModel
    {
        public FeaturedRgbDesignerBase()
        {
        }

        protected void WledChanged()
        {
            AsyncOnPropertyChange(nameof(WledEffectCategory));
        }

        public IEnumerable<WledEffectCategory> WledEffectCategoryList => WledFixture.WledEffectCategoryList;

        public abstract WledEffectCategory WledEffectCategory { get; set; }

        public virtual bool HasFeaturedLedChildren => false;
        public virtual bool HasLegacyLedChildren => false;

    }
    public class FeaturedRgbDesigner : FeaturedRgbDesignerBase
    {
        private readonly RGBLedFixtureCollection rgbLedCollection;

        public string Name => "Featured RGB";

        public override bool HasFeaturedLedChildren => true;
        public override bool HasLegacyLedChildren => LegacyChildrens.Any();

        public FeaturedRgbDesigner(RGBLedFixtureCollection fixtures)
        {
            this.rgbLedCollection = fixtures;
            this.rgbLedCollection.CurrentEffectChanged += ()=> AsyncOnPropertyChange(nameof(SelectedEffect));
            rgbLedCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<WledFixture>().ForEach(o => o.CurrentEffectChanged += WledChanged);
        }

        public ObservableCollection<EffectBase> EffectList => rgbLedCollection.EffectList;

        public override WledEffectCategory WledEffectCategory
        {
            get
            {
                if (rgbLedCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<WledFixture>().GroupBy(o => o.WledEffectCategory).Count() > 1)
                {
                    return WledEffectCategory.off;
                }
                return rgbLedCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<WledFixture>().FirstOrDefault().WledEffectCategory;
            }
            set
            {
                rgbLedCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<WledFixture>().ForEach(f => f.WledEffectCategory = value);

            }
        }

        public IEnumerable<WledRgbDesignerChildren> Childrens
        {
            get
            {
                return rgbLedCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<WledFixture>()
                    .Select(s => new WledRgbDesignerChildren(s));
            }
        }

        public IEnumerable<RGBLedViewModelChildren> LegacyChildrens
        {
            get
            {
                return rgbLedCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<RgbFixture>()
                    .Where(o=> o.GetType() != typeof(WledFixture))
                    .Select(s => new RGBLedViewModelChildren(s));
            }
        }

        public  EffectBase SelectedEffect
        {
            get
            {
                return rgbLedCollection.CurrentEffect;
            }
            set
            {
                rgbLedCollection.CurrentEffect = value;
            }
        }
    }

    public class WledRgbDesignerChildren : FeaturedRgbDesignerBase
    {
        private readonly WledFixture rgbLedCollection;

        public string Name => "Featured RGB";


        public WledRgbDesignerChildren(WledFixture fixtures)
        {
            this.rgbLedCollection = fixtures;
            this.rgbLedCollection.CurrentEffectChanged +=  WledChanged;
        }

        public override WledEffectCategory WledEffectCategory
        {
            get
            {
                return rgbLedCollection.WledEffectCategory;
            }
            set
            {
                rgbLedCollection.WledEffectCategory = value;

            }
        }
    }
}
