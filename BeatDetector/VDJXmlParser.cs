using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJXmlParser
    {
        public Dictionary<string, VDJSong> VDJDatabase = new Dictionary<string, VDJSong>();


        public VDJXmlParser()
        {
            var filename = @"D:\VirtualDJ\database.xml";

            var textReader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XElement purchaseOrder = XElement.Load(textReader);
            var SongNodeList = from item in purchaseOrder.Descendants("Song") select item;
            var allSong = new List<VDJSong>();
            foreach (var songNode in SongNodeList)
            {
                var scanList = from item in songNode.Descendants("Scan") select new VDJScan(item);
                var newSong = new VDJSong(songNode,
                    scanList,
                    from item in songNode.Descendants("Poi") select new VDJPoi(item, scanList.FirstOrDefault()));
                VDJDatabase.Add(newSong.FilePath.Replace("C:", "").Replace("D:", ""), newSong);
            }
        }
    }

}
