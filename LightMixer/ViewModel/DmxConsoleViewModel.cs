using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using LightMixer.Model;
using UIFrameWork;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using radio42.Multimedia.Midi;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections;
using MidiController;
using System.Threading;

namespace LightMixer.ViewModel
{
    public class DmxConsoleViewModel : BaseViewModel
    {
        private int mSelectedChannel = 0;
        private byte mSelectedValue = 0;
        private DmxChaser _chaser;
        private SharedEffectModel _sharedEffectModel;
        private BeatDetector.BeatDetector _BpmDetector;
        private MidiInputDevice _inDevice;
        private MIDIINPROC _midiInProc;
        private IntPtr _midiInHandle;
        private Dispatcher dispatcher;
        public static Process MidiController;


        public DmxConsoleViewModel()
        {
            dispatcher = Dispatcher;
            _chaser = BootStrap.UnityContainer.Resolve<DmxChaser>();
            _BpmDetector = BootStrap.UnityContainer.Resolve<BeatDetector.BeatDetector>();
            _sharedEffectModel = BootStrap.UnityContainer.Resolve<SharedEffectModel>();

            try
            {
                
                try
                {

                    foreach (var pro in Process.GetProcessesByName("LightMixer"))
                    {
                        if (Process.GetCurrentProcess().Id != pro.Id)
                            pro.Kill();
                        Thread.Sleep(2000);
                    }
                    foreach (var pro in Process.GetProcessesByName("MidiController"))
                    {
                        pro.Kill();
                        Thread.Sleep(2000);
                    }
                }
                catch (Exception vepx)
                {

                }
                _midiInProc = new MIDIINPROC(MyMidiInProc);
                MIDIError ret = Midi.MIDI_InOpen(ref _midiInHandle, GetTotalControl(), _midiInProc, IntPtr.Zero, MIDIFlags.MIDI_IO_STATUS);

                if (ret == MIDIError.MIDI_OK)
                {
                    ret = Midi.MIDI_InStart(_midiInHandle);
                    ret = Midi.MIDI_OutReset(_midiInHandle);
                }

                /*MidiController = Process.Start(new ProcessStartInfo
                {
                    FileName = "MidiController.exe",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,

                });
                MidiController.StandardInput.WriteLine(GetTotalControl()+1);
                MidiController.StandardInput.WriteLine(CreateLedMessage(62, true).Message);
                MidiController.StandardInput.WriteLine(CreateResetMessage().Message);*/



            }
            catch (Exception vexp)
            {
                Debug.WriteLine("Could not connect to Total Control");
                //  MessageBox.Show("Could not connect to Total Control");
            }
        }

        private static MidiShortMessage CreateLedMessage(byte note, bool on)
        {
            return new MidiShortMessage
            {
                StatusType = on ?MIDIStatus.NoteOn :  MIDIStatus.NoteOff,
                Velocity = (byte)(on ? 127 : 0),
                ControllerType = MIDIControllerType.User30Fine,
                ControllerValue = 100,
                MessageType = MIDIMessageType.Channel,
                Controller = note,
                Note = note,
                Channel = note,
            };
        }

        private static MidiShortMessage CreateResetMessage()
        {
            return new MidiShortMessage
            {
                StatusType = MIDIStatus.NoteOff,
                Velocity = 0,
                ControllerValue = 100,
                ControllerType = MIDIControllerType.AllNotesOff,
                
                MessageType = MIDIMessageType.Channel,
//                Controller = note,
  //              Note = note,
    //            Channel = note,
            };
        }

        public void AddSysExBuffer(IntPtr handle, int size)
        {
            MIDI_HEADER header = new MIDI_HEADER(size);
            header.Prepare(true, handle);
            if (header.HeaderPtr != IntPtr.Zero)
            {
                Midi.MIDI_InAddBuffer(handle, header.HeaderPtr);
            }
        }

        private void MyMidiInProc(IntPtr handle, MIDIMessage msg, IntPtr instance, IntPtr param1, IntPtr param2)
        {

            // handle all Midi messages here
            if (msg == MIDIMessage.MIM_DATA)
            {
                // process the short message...
                // on Win32 system the param1 and param2 values might be converted like this:
                int p1 = param1.ToInt32();
                int p2 = param2.ToInt32();
                var shortMsg = new MidiShortMessage(param1, param2);
                //Debug.WriteLine("Msg={0} Param1={1} Param2={2}", msg, p1, p2);
                Debug.WriteLine(shortMsg.Channel + " Note:" + shortMsg.Note + " " + shortMsg.Velocity);
                dispatcher.Invoke(() =>
                {
                    ProcessAllMessage(shortMsg);

                });
            }
            else if (msg == MIDIMessage.MIM_LONGDATA)
            {
                // process the system-exclusive message...
                MIDI_HEADER header = new MIDI_HEADER(param1);
                if (header.IsDone)
                {
                    byte[] data = header.Data;
                    Console.WriteLine(header.ToString());
                }
                header.Unprepare(true, handle);

                // add a new buffer
                // since we should constantly provide new buffers until we finished recording
                AddSysExBuffer(handle, 1024);
            }
            else
            {
                /*int p1 = param1.ToInt32();
                int p2 = param2.ToInt32();*/
                //    Debug.WriteLine("Else Msg={0} Param1={1} Param2={2}", msg, p1, p2);
            }
        }

        private void ProcessAllMessage(MidiShortMessage shortMsg)
        {
            ProcessMessageForEncoder(shortMsg, 0, 0, this.Chaser.LedEffectCollection, this.Chaser.CurrentLedEffect, (a) => this.Chaser.CurrentLedEffect = a, 6);
            ProcessMessageForEncoder(shortMsg, 0, 1, this.Chaser.MovingHeadEffectCollection, this.Chaser.CurrentMovingHeadEffect, (a) => this.Chaser.CurrentMovingHeadEffect = a, 6);
            ProcessMessageForEncoder(shortMsg, 0, 3, this.Chaser.BoothEffectCollection, this.Chaser.CurrentBoothEffect, (a) => this.Chaser.CurrentBoothEffect = a, 6);
            ProcessMessageForSlider(shortMsg, 0, 25, 0, this.BeatDetector.BeatRepeat, (a) => this.BeatDetector.BeatRepeat = a, 0.05d);
            ProcessMessageCommand(shortMsg, 0, 67, () => this.ResetBeatCommand.Execute(null));
        }

        private void ProcessMessageCommand(MidiShortMessage shortMsg, int channel, int note, Action p)
        {
            if (shortMsg.Channel == channel && shortMsg.Note == note)
            {
                p();
            }
                        
       //     MidiController.StandardInput.WriteLine(CreateLedMessage(62, false).Message);
        }

        private Dictionary<string, int> Skipper = new Dictionary<string, int>();
        private void ProcessMessageForEncoder<T>(MidiShortMessage shortMsg, int channel, int note, IList<T> collectionSource, T currentItem, Action<T> itemSetter, int skipper)
        {
            if (!UpdateSkipper(shortMsg, channel, note, skipper)) return;
            if (shortMsg.Channel == channel && shortMsg.Note == note)
            {
                var ind = collectionSource.IndexOf(currentItem);
                if (shortMsg.Velocity <= 64)
                {
                    ind++;
                    if (collectionSource.Count == ind)
                    {
                        ind = 0;
                    }
                }
                else
                {
                    ind--;
                    if (ind < 0)
                    {
                        ind = collectionSource.Count - 1;
                    }
                }
                itemSetter(collectionSource[ind]);
            }
        }

        private bool UpdateSkipper(MidiShortMessage shortMsg, int channel, int note, int skipper)
        {
            var chanStr = channel.ToString() + note.ToString();
            if (!Skipper.ContainsKey(chanStr))
            {
                Skipper[chanStr] = 0;
            }
            var currentSkipper = Skipper[chanStr];

            if (currentSkipper >= skipper)
            {
                Skipper[chanStr] = 0;
                return true;
            }
            Skipper[chanStr]++;
            return false;
        }
        private void ProcessMessageForSlider(MidiShortMessage shortMsg, int channel, int note, int skipper, double sliderCurrent, Action<double> itemSetter, double increment)
        {
            if (!UpdateSkipper(shortMsg, channel, note, skipper)) return;

            double ind = 0;
            if (shortMsg.Channel == channel && shortMsg.Note == note)
            {
                if (shortMsg.Velocity <= 64)
                {
                    ind = ind + increment;
                }
                else
                {
                    ind = ind - increment;
                    if (sliderCurrent + ind < 1)
                    {
                        ind = 1;
                    }
                }
                itemSetter(sliderCurrent + ind);
            }
        }

        private static int GetTotalControl()
        {

            var DeviceDescriptions = new List<string>(MidiInputDevice.GetDeviceDescriptions());
            int deviceId = 0;
            foreach (string deviceDescription in DeviceDescriptions)
            {
                // if (deviceDescription.Contains("loopM"))
                if (deviceDescription.Contains("Total"))

                {
                    var foundDevice = MidiInputDevice.GetDeviceDescription(0);
                    return deviceId;
                }
                deviceId++;
            }

            throw new Exception("Midi Device Not Found, can't listen to beat");
        }

        private void OpenAndStartDevice(int device)
        {
            _inDevice = new MidiInputDevice(device);
            _inDevice.AutoPairController = true;
            _inDevice.MessageFilter = MIDIMessageType.SystemRealtime;
            _inDevice.MessageReceived += new MidiMessageEventHandler(InDevice_MessageReceived);

            if (_inDevice.Open())
            {
                if (!_inDevice.Start())
                    Console.WriteLine("Midi device could not be started! Error: " + _inDevice.LastErrorCode.ToString());
            }
            else
                Console.WriteLine("Midi device could not be opened! Error: " + _inDevice.LastErrorCode.ToString());
        }
        private void InDevice_MessageReceived(object sender, MidiMessageEventArgs e)
        {


            Debug.WriteLine(e.ShortMessage?.Note + " " + e.ShortMessage?.Velocity);
        }

        public DmxChaser Chaser
        {
            get
            {
                return _chaser;
            }
        }

        public SharedEffectModel SharedEffectModel
        {
            get
            {
                return _sharedEffectModel;
            }
        }

        public BeatDetector.BeatDetector BeatDetector
        {
            get
            {
                return _BpmDetector;
            }
        }

        public ICommand ResetBeatCommand
        {
            get
            {
                return new DelegateCommand(() => { BeatDetector.BeatRepeat = 1; });
            }
        }
    }
}

