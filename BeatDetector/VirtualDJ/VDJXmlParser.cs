using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJXmlParser
    {
        public Dictionary<string, VDJSong> VDJDatabase = new Dictionary<string, VDJSong>();
        private string filename = @"D:\VirtualDJ\database.xml";

        public VDJXmlParser()
        {
            Reload();
        }

        private XDocument Load(FileStream textReader, Dictionary<string, VDJSong> dictionary)
        {
            var xDoc = XDocument.Load(textReader, LoadOptions.PreserveWhitespace);
            XElement purchaseOrder = xDoc.Root;
            var SongNodeList = from item in purchaseOrder.Descendants("Song") select item;
            var allSong = new List<VDJSong>();
            foreach (var songNode in SongNodeList)
            {
                var scanList = from item in songNode.Descendants("Scan") select new VDJScan(item);
                var newSong = new VDJSong(songNode,
                    scanList,
                    from item in songNode.Descendants("Poi").Where( o=>o.Attribute("Type")?.Value == "remix") select new VDJPoi(item, scanList.FirstOrDefault()));
                dictionary.Add(newSong.FilePath.Replace("C:", "").Replace("D:", ""), newSong);
            }
            return xDoc;
        }

        public void Save()
        {
            try
            {
                var textReader = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                var newEntry = new Dictionary<string, VDJSong>();
                var xdoc = Load(textReader, newEntry);

                foreach (var oldEntry in VDJDatabase.Values)
                {
                    foreach (var pois in oldEntry.Pois)
                    {
                        
                        if (pois.IsNew)
                        {
                            var corespondingElement = newEntry.SingleOrDefault(o => o.Value.FilePath == oldEntry.FilePath).Value;
                            AddPoi(corespondingElement, pois);
                            pois.IsNew = false;
                        }
                        if (pois.IsDeleted)
                        {
                            var corespondingElement = newEntry.SingleOrDefault(o => o.Value.FilePath == oldEntry.FilePath).Value;
                            //  <Poi Name="Break 1" Pos="13.823129" Type="remix" />
                            DeletePOI(corespondingElement, pois);
                            pois.IsDeleted = false;
                        }
                    }
                }
                
                textReader.Close();
                xdoc.Save(filename);
            }
            catch(Exception vexp)
            {
                MessageBox.Show(vexp.ToString());
            }
        }

        private void AddPoi(VDJSong corespondingElement, VDJPoi pois)
        {
            XAttribute[] attArray = {
                new XAttribute("Name", pois.Name),
                new XAttribute("Pos", pois.Pos),
                new XAttribute("Type", "remix")
            };
            var newPoi = new XElement("Poi", attArray);
            corespondingElement.XmlNode.Add(newPoi);
        }

        private void DeletePOI(VDJSong corespondingElement, VDJPoi pois)
        {
            var elementToDelete = corespondingElement.Pois.Where(poisToCompare => poisToCompare.Name == pois.Name &&
                poisToCompare.Type == pois.Type &&
                poisToCompare.Pos == pois.Pos).FirstOrDefault();
            if (elementToDelete != null)
            {
                elementToDelete.Source?.Remove();
                
            }

        }

        public void Reload()
        {
            var textReader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var newEntry = new Dictionary<string, VDJSong>();
            Load(textReader, newEntry);
            textReader.Close();
            VDJDatabase = newEntry;
            LastUpdated = DateTime.Now;
        }
        private DateTime LastUpdated = DateTime.Now;

        public void CheckForRefresh()
        {
            if (( DateTime.Now - LastUpdated).TotalSeconds > 5)
            {
                FileInfo fInfo = new FileInfo(filename);
                if (LastUpdated < fInfo.LastWriteTime)
                {
                    LastUpdated = DateTime.Now.AddSeconds(10);
                    Task.Factory.StartNew(() =>
                    {
                        Reload();
                    });
                }
            }
        }
    }

}
