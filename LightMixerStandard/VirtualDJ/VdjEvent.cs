using System;
using System.Linq;

namespace BeatDetector
{
    public class OS2lEvent
    {
        public double BeatPos { get; set; }
        public double Bpm { get; set; }
        public double Elapsed { get; set; }
    }

    public class VdjEvent
    {
        public VDJSong VDJSong { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        

        private DateTime elapsedsettime = DateTime.Now;
        public string Elapsed
        {
            get
            {
                return elapsed;
            }

            internal set
            {
                elapsed = value;
                elapsedsettime = DateTime.Now;
            }
        }
        public string BPM { get; internal set; }

        private static VDJPoi DefaultPOI = new VDJPoi();
        private string elapsed;

        public double BpmAsDouble
        {
            get
            {
                double bpm = 1;
                double.TryParse(BPM, out bpm);
                return bpm;
            }
        }

        public long Position
        {
            get
            {
                double elapsedMs = 0;
                double bpm = 1;
                double.TryParse(Elapsed, out elapsedMs);
                double.TryParse(BPM, out bpm);
                long position = Convert.ToInt64(elapsedMs * bpm);
                return position;
            }
        }

        public bool IsPoiPlausible
        {
            get
            {
                return this.VDJSong?.AutomatedPois?.ToArray().Any() == true || this.VDJSong?.ZPlanePois?.ToArray().Length >= 3 || this.VDJSong?.Pois?.Count >= 3 || GetCurrentPoi.ID == 0;
            }
        }

        public VDJPoi GetCurrentPoi
        {
            get
            {
                return GetPoisAtPosition(this.Position);
            }
        }

        public VDJPoi GetPoisAtPosition(long position)
        {
            if (VDJSong?.UseAutomation == true)
            {
                return this.VDJSong?.AutomatedPois?
                    .ToArray()
                    .Where(o => position > o.Position)
                    .OrderBy(o => o.Position)
                    .LastOrDefault() ?? DefaultPOI;
            }
            else
            {
                VDJPoi currentPoi = null;
                if (VDJSong?.UseZPlane == true || this.VDJSong?.VDJPoiPlausible !=true)
                {
                    currentPoi = this.VDJSong?.ZPlanePois?
                            .ToArray()
                            .Where(o => position > o.Position && o.Type == "Zplane")
                            .OrderBy(o => o.Position)
                            .LastOrDefault();
                }

                if (currentPoi == null)
                {
                    currentPoi = this.VDJSong?.Pois
                            .Where(o => position > o.Position && o.Type == "remix")
                            .OrderBy(o => o.Position)
                            .LastOrDefault();
                }
                if (currentPoi != null)
                {
                    return currentPoi;
                }
            }
            return DefaultPOI;
        }

        public VDJPoi GetNextPoi
        {
            get
            {
                return GetNextPoiBasedOnPosition(Position);
            }
        }

        public VDJPoi GetNextPoiBasedOnPosition(long position)
        {
            VDJPoi currentPoi = this.VDJSong?.ZPlanePois?
                                    .Where(o => position < o.Position && o.Type == "Zplane")
                                    .OrderBy(o => o.Position)
                                    .FirstOrDefault();

            if (currentPoi == null)
            {
                currentPoi = this.VDJSong?.Pois
                    .Where(o => position < o.Position && o.Type == "remix")
                    .OrderBy(o => o.Position)
                    .FirstOrDefault();
            }
            if (currentPoi != null)
            {
                return currentPoi;
            }
            return DefaultPOI;
        }

        public double GetEffectiveVolume
        {
            get
            {
                var effectiveCrossFader = CrossFader;

                return Volume * CrossFader;
            }
        }

        public double CrossFader { get; internal set; }
        public double BeatNumber { get; internal set; }
        public double BeatBar16 { get; internal set; }
        public double BeatBar { get; internal set; }
        public double BeatPos { get; set; }
        public double BeatGrid { get; internal set; }
        public double Volume { get; internal set; }
        public int Deck { get; internal set; }

        public override string ToString()
        {
            return "Deck " + Deck + ", Playing: " + FileName;
        }
    }
}