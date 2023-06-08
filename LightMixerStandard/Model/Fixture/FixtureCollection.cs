using LightMixer.Model.Fixture;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LightMixer.Model
{
    public class FixtureCollection
    {
        public Func<double> intensityGetter;
        public Func<double> intensityFlashGetter;
        private EffectBase currentEffect;

        public event CurrentEffectChangedEventHandler CurrentEffectChanged;

        public delegate void CurrentEffectChangedEventHandler();

        public ObservableCollection<EffectBase> EffectList { get; } = new ObservableCollection<EffectBase>();

        public FixtureCollection()
        {
            EffectList.CollectionChanged += EffectList_CollectionChanged;
            this.intensityFlashGetter = () => 0.8;
            this.intensityGetter = () => 0.8;
        }

        public void BindCollectionToIntensityGetter(Func<double> intensityGetter, Func<double> intensityFlashGetter)
        {
            this.intensityGetter = intensityGetter;
            this.intensityFlashGetter = intensityFlashGetter;
        }

        private void EffectList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (EffectBase newItem in e.NewItems)
                {
                    newItem.Owner = this;
                }
            }
        }

        public EffectBase CurrentEffect
        {
            get
            {
                if (currentEffect == null && EffectList.Any())
                {
                    return EffectList.First();
                }
                return currentEffect;
            }
            set
            {
                var candidate = value;
                if (!EffectList.Contains(value))
                {
                    candidate = EffectList.FirstOrDefault(o => o?.Name == value?.Name);
                    if (candidate == null || candidate.Name == null)
                        throw new System.Exception("Cannot set an effect not in the collection");
                }
                currentEffect = candidate;
                CurrentEffectChanged?.Invoke();
            }
        }

        public List<FixtureGroup> FixtureGroups { get; set; } = new List<FixtureGroup>();
    }
}