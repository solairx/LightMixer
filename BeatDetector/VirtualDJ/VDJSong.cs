using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJSong
    {
        public XElement XmlNode;
        public bool ZplaneLoad { get; set; } = false;

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

        public SortableObservableCollection<VDJPoi> ZPlanePois { get; set; }

        public override string ToString()
        {
            return FilePath;
        }

        internal void LoadZplace()
        {
            if (File.Exists(FilePath + ".json"))
            {
                Task.Factory.StartNew(() =>
                {
                    var fileContent = File.ReadAllText(FilePath + ".json");

                    var json = JsonConvert.DeserializeObject<IEnumerable<ZPlanePOI>>(fileContent) ;
                    var order = json.Select(o => o.G)
                    .Distinct()
                    .OrderBy(o => o)
                    .ToList();


                    var zplaneList = new List<ZplanePoi>();

                    foreach (var zplaneElement in json)
                    {
                        var total = zplaneElement.Start.TotalMilliseconds / 1000;

                        var vdjpois = new ZplanePoi(order.IndexOf(zplaneElement.G), order.Count(), zplaneElement.G, zplaneElement.B,
                            total.ToString(),
                            Scans.First());
                        {
                        };
                        zplaneList.Add(vdjpois);
                    }

                    var zplanePois = new SortableObservableCollection<VDJPoi>(zplaneList);
                    zplanePois.Sort(o => o.Position, System.ComponentModel.ListSortDirection.Ascending);
                    ZPlanePois = zplanePois;
                    ZplaneLoad = true;
                    //write string to file
                });

            }
        }

        public class ZPlanePOI
        {
            public int R { get; set; }
            public int G { get; set; }
            public int B { get; set; }
            public TimeSpan Start { get; set; }
            public TimeSpan Stop { get; set; }
        }
    }
}