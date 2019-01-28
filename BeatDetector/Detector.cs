using radio42.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;
using Un4seen.Bass.Misc;

namespace BeatDetector
{
    public class BeatDetector : INotifyPropertyChanged
    {
        private int mStream = 0;
        private RECORDPROC _myRecProc;
        private MidiInputDevice _inDevice;
        private DateTime lastBeatRunned;

        private int SliderPosition;
        private int VelocityDeckA;
        private int VelocityDeckB;

        private bool DeckAIsBeat;
        private bool DeckBIsBeat;

        public event BeatHandler BeatEvent;
        public delegate void BeatHandler(bool Beat, object caller);

        public event BpmHandler BpmEvent;
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void BpmHandler(double Beat, object caller);

        private double _beatRepeat = 1;
        private Brush _beatBackground;
        private Brush _blackColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private Brush _redColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));


        public double BeatRepeat
        {
            get
            {
                return _beatRepeat;
            }
            set
            {
                _beatRepeat = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(BeatRepeat)));
                }
                if (value > 1.05 || value < 0.95)
                {
                    BeatBackground = _redColor;
                }
                else
                {
                    BeatBackground = _blackColor;
                }
            }
        }

        public Brush BeatBackground
        {
            get
            {
                return _beatBackground;
            }
            set
            {
                _beatBackground = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(BeatBackground)));
                }
                
            }
        }


        public BeatDetector()
        {
            _beatBackground = _blackColor;
            this.OpenAndStartDevice(GetLoopBeDeviceId());

            BeatRepeatProc();

        }

        private static int GetLoopBeDeviceId()
        {
            var DeviceDescriptions = new List<string>(MidiInputDevice.GetDeviceDescriptions());
            int deviceId = 0;
            foreach (string deviceDescription in DeviceDescriptions)
            {
                if (deviceDescription.Contains("LoopBe"))
                //if (deviceDescription.Contains("loopMIDI Port"))

                {
                    var foundDevice = MidiInputDevice.GetDeviceDescription(0);
                    return deviceId;
                }
                deviceId++;
            }

            throw new Exception("Midi Device Not Found, can't listen to beat");
        }

        // open and start a certain Midi input device 
        private void OpenAndStartDevice(int device)
        {
            _inDevice = new MidiInputDevice(device);
            _inDevice.AutoPairController = true;
            _inDevice.MessageFilter = MIDIMessageType.SystemRealtime | MIDIMessageType.SystemExclusive;
            _inDevice.MessageReceived += new MidiMessageEventHandler(InDevice_MessageReceived);

            if (_inDevice.Open())
            {
                if (!_inDevice.Start())
                    Console.WriteLine("Midi device could not be started! Error: " + _inDevice.LastErrorCode.ToString());
            }
            else
                Console.WriteLine("Midi device could not be opened! Error: " + _inDevice.LastErrorCode.ToString());
        }


        private void StopAndCloseDevice()
        {
            if (_inDevice != null && _inDevice.IsStarted)
            {
                _inDevice.Stop();
                _inDevice.Close();
                _inDevice.MessageReceived -= new MidiMessageEventHandler(InDevice_MessageReceived);
            }
        }
        private void InDevice_MessageReceived(object sender, MidiMessageEventArgs e)
        {

            try
            {
                double currentBpm = 130;
                if (BpmEvent != null)
                    BpmEvent(130, this);

                int tracktorBeatMin = 50;// was 20 for traktor
                int tracktorBeatMax = 85;// was 20 for traktor
                if (e.IsShortMessage)
                {
                    // Console.WriteLine("Midi Note:" + e.ShortMessage.Note + " value:" + e.ShortMessage.Velocity);

                    if (e.ShortMessage.Channel == 0 && e.ShortMessage.Note == 2)
                        SliderPosition = e.ShortMessage.Velocity;
                    if (e.ShortMessage.Channel == 0 && e.ShortMessage.Note == 1)
                    {
                        VelocityDeckB = e.ShortMessage.Velocity;
                        if (SliderPosition > 60)
                        {
                            if (VelocityDeckB < tracktorBeatMax && VelocityDeckB > tracktorBeatMin)
                            {
                                if (!DeckBIsBeat)
                                {
                                    BeatEvent(false, this);
                                    lastBeatRunned = DateTime.Now;
                                }
                                DeckBIsBeat = true;
                            }
                            else
                            {
                                if (DeckBIsBeat)
                                {
                                    BeatEvent(true, this);
                                    lastBeatRunned = DateTime.Now;
                                }
                                DeckBIsBeat = false;
                            }
                        }
                    }
                    if (e.ShortMessage.Channel == 0 && e.ShortMessage.Note == 0)
                    {

                        VelocityDeckA = e.ShortMessage.Velocity;
                        if (SliderPosition < 70)
                        {
                            if (VelocityDeckA < tracktorBeatMax && VelocityDeckA > tracktorBeatMin)
                            {
                                if (!DeckAIsBeat)
                                {
                                    BeatEvent(false, this);
                                    lastBeatRunned = DateTime.Now;
                                }
                                DeckAIsBeat = true;
                            }
                            else
                            {
                                if (DeckAIsBeat)
                                {
                                    BeatEvent(true, this);
                                    lastBeatRunned = DateTime.Now;
                                }
                                DeckAIsBeat = false;
                            }
                        }
                    }
                    int virtualDjBeat = 75;// was 20 for traktor
                    if (e.ShortMessage.Channel == 0 && e.ShortMessage.Note == 3)
                    {
                        VelocityDeckB = e.ShortMessage.Velocity;
                        if (SliderPosition < 70)
                        {
                            if (VelocityDeckB < virtualDjBeat)
                            {
                                if (!DeckBIsBeat)
                                {
                                    BeatEvent(false, this);
                                    lastBeatRunned = DateTime.Now;
                                }
                                DeckBIsBeat = true;
                            }
                            else
                            {
                                if (DeckBIsBeat)
                                {
                                    BeatEvent(true, this);
                                    lastBeatRunned = DateTime.Now;
                                }
                                DeckBIsBeat = false;
                            }
                        }
                    }
                    if (e.ShortMessage.Channel == 0 && e.ShortMessage.Note == 4)
                    {

                        VelocityDeckA = e.ShortMessage.Velocity;

                        if (SliderPosition > 60)
                        {
                            if (VelocityDeckA < virtualDjBeat)
                            {
                                if (!DeckAIsBeat)
                                {
                                    BeatEvent(false, this);
                                    lastBeatRunned = DateTime.Now;
                                }
                                DeckAIsBeat = true;
                            }
                            else
                            {
                                if (DeckAIsBeat)
                                {
                                    BeatEvent(true, this);
                                    lastBeatRunned = DateTime.Now;
                                }
                                DeckAIsBeat = false;
                            }
                        }
                    }



                    Console.WriteLine("{0} : {1}", e.ShortMessage.ID, e.ShortMessage.ToString());
                }
                else if (e.IsSysExMessage)
                {
                    Console.WriteLine("{0} : {1}", e.SysExMessage.ID, e.SysExMessage.ToString());
                }
                else if (e.EventType == MidiMessageEventType.Opened)
                {
                    //Console.WriteLine("Midi device {0} opened.", e.DeviceID);
                }
                else if (e.EventType == MidiMessageEventType.Closed)
                {
                    Console.WriteLine("Midi device {0} closed.", e.DeviceID);
                }
                else if (e.EventType == MidiMessageEventType.Started)
                {
                    Console.WriteLine("Midi device {0} started.", e.DeviceID);
                }
                else if (e.EventType == MidiMessageEventType.Stopped)
                {
                    Console.WriteLine("Midi device {0} stopped.", e.DeviceID);
                }

            }
            catch (Exception vexp)
            {
            }
        }

        private void BeatRepeatProc()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(15);
                    if (BeatRepeat > 1.25)
                    {
                        if (lastBeatRunned.AddSeconds(1 / (130 /*130 is bpm ?*/ * BeatRepeat / 60)).Ticks < DateTime.Now.Ticks)
                        {
                            if (_beatRepeat > 1.25 || _beatRepeat < 0.75)
                            {
                                if (BeatEvent != null)
                                    BeatEvent(true, this);
                                lastBeatRunned = DateTime.Now;
                            }
                        }
                    }
                }
            });
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
