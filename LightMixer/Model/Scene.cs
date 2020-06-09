using System.Collections.Generic;

namespace LightMixer.Model
{
    public class Scene
    {
        public string Name { get; set; }
        public List<Zone> Zones { get; set; }

        public Scene()
        {
            this.Zones = new List<Zone>();
        }
    }
}


