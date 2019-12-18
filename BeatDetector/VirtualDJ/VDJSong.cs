using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJSong
    {
        public XElement XmlNode;
        public VDJSong(XElement source, IEnumerable<VDJScan> scanList, IEnumerable<VDJPoi> poiList)
        {
            XmlNode = source;
            FilePath = (string)source.Attribute("FilePath");
            Scans = scanList;
            Pois = new SortableObservableCollection<VDJPoi>(poiList);
            Pois.Sort(o => o.Position, System.ComponentModel.ListSortDirection.Ascending);
        }
        public string FilePath { get; }

        public IEnumerable<VDJScan> Scans { get; }
        public SortableObservableCollection<VDJPoi> Pois { get; }

        public override string ToString()
        {
            return FilePath;
        }
    }

}
