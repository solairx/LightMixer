using System;
using System.Linq;

namespace BeatDetector
{
    public class VdjEvent
    {
        public VDJSong VDJSong { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }
        public string Elapsed { get; internal set; }
        public string BPM { get; internal set; }

        private static VDJPoi DefaultPOI = new VDJPoi();

        public double BpmAsDouble
        {
            get
            {
                double bpm = 1;
                double.TryParse(BPM, out bpm);
                return bpm ;
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

        public VDJPoi GetCurrentPoi
        {
            get
            {
                var currentPoi = this.VDJSong?.Pois
                        .Where(o => Position > o.Position && o.Type == "remix")
                        .OrderBy(o => o.Position)
                        .LastOrDefault();
                if (currentPoi != null)
                {
                    return currentPoi;
                }
                return DefaultPOI;
            }
        }

        public VDJPoi GetNextPoi
        {
            get
            {
                var currentPoi = this.VDJSong?.Pois
                        .Where(o => Position < o.Position && o.Type == "remix")
                        .OrderBy(o => o.Position)
                        .FirstOrDefault();
                if (currentPoi != null)
                {
                    return currentPoi;
                }
                return DefaultPOI;
            }
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
        public double BeatPos { get; internal set; }
        public double Volume { get; internal set; }
        public int Deck { get; internal set; }

        public override string ToString()
        {
            return "Deck " + Deck + ", Playing: " + FileName;
        }
    }

}
