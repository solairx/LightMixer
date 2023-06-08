using LightMixer.Model;
using LightMixer.Model.Fixture;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UIFrameWork;

namespace LightMixer.View
{
    public class RGBLedViewModel : RGBLedViewModelBase
    {
        private readonly RGBLedFixtureCollection rgbLedCollection;

        public string Name => "RGB Led";

        public virtual bool HasChildren => true;

        public RGBLedViewModel(RGBLedFixtureCollection fixtures)
        {
            this.rgbLedCollection = fixtures;
            this.rgbLedCollection.CurrentEffectChanged += () => AsyncOnPropertyChange(nameof(SelectedEffect));
        }

        public ObservableCollection<EffectBase> EffectList => rgbLedCollection.EffectList;

        public EffectBase SelectedEffect
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

        public IEnumerable<RGBLedViewModelChildren> Childrens
        {
            get
            {
                return rgbLedCollection.FixtureGroups.SelectMany(o => o.FixtureInGroup).OfType<RgbFixture>()
                    .Select(s => new RGBLedViewModelChildren(s));
            }
        }
    }

    public abstract class RGBLedViewModelBase : BaseViewModel
    {
        public virtual bool HasChildren => false;
    }

    public class RGBLedViewModelChildren : BaseViewModel
    {
        private readonly RgbFixture rgbLedCollection;

        public virtual bool HasChildren => false;

        public string Name => "RGB Led";


        public RGBLedViewModelChildren(RgbFixture fixtures)
        {
            this.rgbLedCollection = fixtures;
            //this.rgbLedCollection.cu += () => AsyncOnPropertyChange(nameof(SelectedEffect));
        }
       
    }
}
