using BeatDetector;
using System.Collections.Generic;
using System.Linq;

namespace LightMixer.Model
{
    public class ActiveDeckSelector
    {
        public IEnumerable<VdjEvent> Select(ICollection<VdjEvent> values)
        {
            return values.OrderByDescending(vdjEvent => vdjEvent.GetEffectiveVolume);
        }
    }
}
