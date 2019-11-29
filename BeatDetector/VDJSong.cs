using System.Collections.Generic;
using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJSong
    {
        public VDJSong(XElement source, IEnumerable<VDJScan> scanList, IEnumerable<VDJPoi> poiList)
        {
            FilePath = (string)source.Attribute("FilePath");
            Scans = scanList;
            Pois = poiList;
        }
        public string FilePath { get; }

        public IEnumerable<VDJScan> Scans { get; }
        public IEnumerable<VDJPoi> Pois { get; }

        public override string ToString()
        {
            return FilePath;
        }
    }

}
