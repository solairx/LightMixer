using System;
using System.Linq;
using System.Xml.Linq;

namespace BeatDetector
{
    public class VDJPoi : ViewModelBase
    {
        private readonly VDJScan vDJScan;
        private bool isCurrent;
        public XElement Source;

        public VDJPoi(string name, string pos, string type, VDJScan vDJScan)
        {
            this.vDJScan = vDJScan;
            Name = name;
            Pos = pos;
            Type = type;
        }
        public VDJPoi() { }
        public VDJPoi(XElement source, VDJScan vDJScan)
        {
            Source = source;
            Name = (string)source.Attribute("Name");
            Pos = (string)source.Attribute("Pos");
            Type = (string)source.Attribute("Type");
            this.vDJScan = vDJScan;
            IsDeleted = false;
            IsNew = false;
        }
        public string Name { get; }
        public string Pos { get; }
        public string Type { get; }

        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }

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

        public bool IsCurrent
        {
            get => isCurrent;
            set
            {
                isCurrent = value;
                OnPropertyChanged(nameof(IsCurrent));
            }

        }

        public bool IsNull 
        { get
            {
                return Position == 1;
            }
        }

        public override bool Equals(object obj)
        {
            var poisToCompare = obj as VDJPoi;
            if (poisToCompare == null)
            {
                return base.Equals(obj);
            }
            return poisToCompare.Name == Name &&
                poisToCompare.Type == Type &&
                poisToCompare.Pos == Pos; ;
        }
    }

}
