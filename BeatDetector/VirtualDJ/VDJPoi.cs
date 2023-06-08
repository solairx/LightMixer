using LightMixer.Model;
using System;
using System.Linq;
using System.Xml.Linq;
using static BeatDetector.VDJSong;

namespace BeatDetector
{
    public class AutomatedPoi : VDJPoi
    {
        public AutomatedPoi(AutomatedPOIJson json, VDJSong song) : base(json.AutomationEnum.ToString(), json.Position.ToString(), "remix", song?.Scans?.FirstOrDefault())
        {
            this.json = json;

            Automation = json.AutomationEnum;

        }

        public override bool IsBreak => false;
        public override bool IsEndBreak => false;

        public string Automation { get; set; }

        public override string Name => Automation.ToString();

        public AutomatedPOIJson json { get; private set; }
    }

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

        public VDJPoi(string name, TimeSpan pos, VDJScan vDJScan)
        {
            this.vDJScan = vDJScan;
            Name = name;
            Pos = (pos.TotalMilliseconds/1000).ToString();
            Type = "ML";
            RawPosition = pos;
        }

        public TimeSpan RawPosition { get; set; }

        public VDJPoi()
        {
            
        }

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
        /// <summary>
        /// For generation of vdjPOIS when some are missing, ex BREAK BREAK ENDBREAK, a endbreak is missing
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="vDJScan"></param>
        public VDJPoi(string name, double pos, VDJScan vDJScan)
        {
            Name = name;
            Pos = pos.ToString();
            Type = "remix";
            this.vDJScan = vDJScan;
            IsDeleted = false;
            IsNew = false;
        }

        public virtual string Name { get; }
        public string Pos { get; }
        public double PosAsDouble => double.Parse(Pos);

        public string PosInSecond
        {
            get
            {
                Double dblPos = -1;
                Double.TryParse(this.Pos, out dblPos);
                var ts = TimeSpan.FromSeconds(dblPos);
                if (ts.Hours > 0)
                {
                    return ts.Hours + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00") + ":" + ts.Milliseconds.ToString("00");
                }
                else
                {
                    return ts.Minutes + ":" + ts.Seconds.ToString("00") + ":" + ts.Milliseconds.ToString("00");
                }

            }
        }
        public string Type { get; }

        public bool IsDeleted { get; set; }
        public bool IsNew { get; set; }

        public virtual bool IsBreak
        {
            get
            {
                return Name?.Contains("Break")== true && !IsEndBreak;
            }
        }

        public virtual bool IsEndBreak
        {
            get
            {
                return Name?.Contains("End Break") == true;
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
                OnPropertyChanged(nameof(IsCurrentAndNew));
            }
        }

        public bool IsCurrentAndNew => isCurrent && IsNew;
        

        public bool IsNull
        {
            get
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