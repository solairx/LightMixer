using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJScan
    {
        public VDJScan(XElement source)
        {
            AltBpm = (string)source.Attribute("AltBpm");
            Bpm = (string)source.Attribute("Bpm");
            Key = (string)source.Attribute("Key");
        }

        public string Bpm { get; }
        public string AltBpm { get; }
        public string Key { get; }
    }
}