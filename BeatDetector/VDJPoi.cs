using System;
using System.Linq;
using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJPoi
    {
        private readonly VDJScan vDJScan;

        public VDJPoi() { }
        public VDJPoi(XElement source, VDJScan vDJScan)
        {
            Name = (string)source.Attribute("Name");
            Pos = (string)source.Attribute("Pos");
            Type = (string)source.Attribute("Type");
            this.vDJScan = vDJScan;

        }
        public string Name { get; }
        public string Pos { get; }
        public string Type { get; }

        public bool IsBreak
        {
            get
            {
                return Name.Contains("Break") && !IsEndBreak;
            }
        }

        public bool IsEndBreak
        {
            get
            {
                return Name.Contains("End Break");
            }
        }

        public int ID
        {
            get
            {
                int id = 0;
                if (!string.IsNullOrWhiteSpace(Name))
                {

                    int.TryParse(Name.Split(' ').Last(), out id);
                }
                return id;
            }
        }
        public long Position
        {
            get
            {
                if (vDJScan != null)
                {
                    double bpm = 1;
                    double.TryParse(vDJScan.Bpm, out bpm);
                    if (bpm != 0)
                    {
                        bpm = 60 / bpm;
                    }
                    double elapsedMs = 0;
                    double.TryParse(Pos, out elapsedMs);
                    return Convert.ToInt64(elapsedMs * bpm * 1000);
                }
                return 1;
            }
        }
    }

}
