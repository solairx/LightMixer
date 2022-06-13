using System;
using Un4seen.Bass;
using Un4seen.Bass.Misc;

namespace BeatDetector
{
    public class WaveInBeatDetector
    {
        private int mStream = 0;
        private RECORDPROC _myRecProc;

        private DateTime lastBeatRunned;

        public event BeatHandler BeatEvent;
        public delegate void BeatHandler(bool Beat, object caller);

        public event BpmHandler BpmEvent;
        public delegate void BpmHandler(double Beat, object caller);

        private double _beatRepeat = 1;

        public double BeatRepeat
        {
            get
            {
                return _beatRepeat;
            }
            set
            {
                _beatRepeat = value;
            }
        }

        BPMCounter counter = new BPMCounter(10, 44100);
        public double bpm;
        public bool beat = false;
        public double mbeatpos;
        public float mpercent;
        public WaveInBeatDetector()
        {

            if (Bass.BASS_RecordInit(-1))
            {
                _myRecProc = new RECORDPROC(MyRecording);
                counter.BPMHistorySize = 10;
                counter.MaxBPM = 200;
                counter.MinBPM = 40;
                int mStream = Bass.BASS_RecordStart(44100, 2, BASSFlag.BASS_RECORD_PAUSE, 10, _myRecProc, IntPtr.Zero);
                Bass.BASS_ChannelPlay(mStream, false);
            }
            else
            {
                throw new Exception("Cannot start recording");
            }


        }


        private bool MyRecording(int handle, IntPtr buffer, int length, IntPtr user)
        {

            double currentBpm = 60;
            bool newBeat = counter.ProcessAudio(handle, false);

            if (counter.BPM < 180 && counter.BPM > 60)
            {
                currentBpm = counter.BPM;
            }

            if (newBeat != beat && BeatEvent != null)
            {
                if (BeatEvent != null)
                    BeatEvent(beat, this);
                lastBeatRunned = DateTime.Now;
            }
            else if (lastBeatRunned.AddSeconds(1 / (currentBpm * BeatRepeat / 60)).Ticks < DateTime.Now.Ticks)
            {
                if (BeatEvent != null)
                    BeatEvent(true, this);
                lastBeatRunned = DateTime.Now;
            }


            if (this.bpm != currentBpm && BeatEvent != null)
                BpmEvent(currentBpm, this);

            this.beat = newBeat;
            this.bpm = counter.BPM;
            return true;
        }

        public void Stop()
        {
            // free the stream
            Bass.BASS_StreamFree(mStream);
            // free BASS
            Bass.BASS_Free();
        }
    }
}
