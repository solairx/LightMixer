using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJXmlParser
    {
        public Dictionary<string, VDJSong> VDJDatabase = new Dictionary<string, VDJSong>();

        //       private string filename = @"\\Desktop-pjdgjgm\d\VirtualDJ\database.xml";
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
                    from item in songNode.Descendants("Poi").Where(o => o.Attribute("Type")?.Value == "remix") select new VDJPoi(item, scanList.FirstOrDefault()));
                InterpolateMissingPoisFromVDJDB(songNode, scanList, newSong);

                dictionary.Add(newSong.FilePath.Replace("C:", "").Replace("D:", ""), newSong);
            }
            return xDoc;
        }

        private void InterpolateMissingPoisFromVDJDB(XElement songNode, IEnumerable<VDJScan> scanList, VDJSong newSong)
        {
            var energyKeyValuePair = from item in songNode.Descendants("Poi").Where(o => o.Attribute("Type")?.Value == "cue" && o.Attribute("Name")?.Value?.Contains("Energy") == true)
                                     select new KeyValuePair<double, int>(Double.Parse(item.Attribute("Pos")?.Value ?? "0"), Int32.Parse(item.Attribute("Name").Value.Split('y')[1]));
            if (energyKeyValuePair.Count() > 2)
            {
                var min = energyKeyValuePair.Min(o => o.Value);
                var max = energyKeyValuePair.Max(o => o.Value);
                var avg = (min + max) / 2;

                if (newSong.Pois.Count > 1 && avg > 4)
                {
                    var candidateList = new List<KeyValuePair<string, KeyValuePair<double, int>>>();
                    var last = newSong.Pois.First();
                    foreach (var vdjPois in newSong.Pois.Skip(1))
                    {
                        var energyPoisInBtw = energyKeyValuePair
                            .Where(o => o.Key <= vdjPois.PosAsDouble && o.Key >= last.PosAsDouble)
                            .OrderBy(o => o.Key);
                        if (vdjPois.IsBreak && last.IsBreak)
                        {
                            // double break found
                            // ex BREAK BREAK ENDBREAK
                            // need to find backward the highest energy and generate POIS for EndBreak
                            var candidate = energyPoisInBtw.LastOrDefault(o => o.Value > avg);
                            if (energyPoisInBtw.Any(o => o.Value > avg))
                            {
                                candidateList.Add(new KeyValuePair<string, KeyValuePair<double, int>>("End Break ", energyPoisInBtw.LastOrDefault(o => o.Value > avg)));
                            }
                        }
                        else if (!vdjPois.IsBreak && !last.IsBreak)
                        {
                            // double end break found
                        }
                        last = vdjPois;
                    }
                    if (candidateList.Any())
                    {
                        foreach (var candidate in candidateList)
                        {
                            newSong.Pois.Add(new VDJPoi(candidate.Key, candidate.Value.Key, scanList.FirstOrDefault()));
                        }
                        newSong.Sort();
                    }
                }
            }
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
            catch (Exception vexp)
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
            try
            {
                if (!File.Exists(filename))
                {
                    MessageBox.Show("Unable to open VirtualDJ Database, file does not exist on " + filename);
                    return;
                }
                var newEntry = new Dictionary<string, VDJSong>();
                var textReader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Load(textReader, newEntry);
                textReader.Close();
                VDJDatabase = newEntry;
                LastUpdated = DateTime.Now;
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to open VirtualDJ Database");
            }
        }

        private DateTime LastUpdated = DateTime.Now;

        public void CheckForRefresh()
        {
            if ((DateTime.Now - LastUpdated).TotalSeconds > 5)
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