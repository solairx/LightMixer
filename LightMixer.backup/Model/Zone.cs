using System.Collections.Generic;

namespace LightMixer.Model
{
    public class Zone
    {
        public string Name { get; set; }
        public List<FixtureCollection> FixtureTypes { get; set; } = new List<FixtureCollection>();
    }
}