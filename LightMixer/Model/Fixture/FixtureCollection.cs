using LightMixer.Model.Fixture;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public class FixtureCollection
    {
        private EffectBase currentEffect;

        public event CurrentEffectChangedEventHandler CurrentEffectChanged;
        public delegate void CurrentEffectChangedEventHandler();

        public List<EffectBase> EffectList { get; } = new List<EffectBase>();
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


