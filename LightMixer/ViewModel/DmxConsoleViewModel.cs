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

namespace LightMixer.ViewModel
{
    public class DmxConsoleViewModel : BaseViewModel  
    {
        private int mSelectedChannel = 0;
        private byte mSelectedValue = 0;
        private DmxChaser _chaser ;
        private SharedEffectModel _sharedEffectModel;
        private BeatDetector.BeatDetector _BpmDetector;
        private MidiInputDevice _inDevice;
        private MIDIINPROC _midiInProc;
        private IntPtr _midiInHandle;
        public DmxConsoleViewModel()
        {
            _chaser = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<DmxChaser>();
            _BpmDetector = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<BeatDetector.BeatDetector>();
            _sharedEffectModel = ((LightMixer.App)LightMixer.App.Current).UnityContainer.Resolve<SharedEffectModel>();

            try
            {


                this.OpenAndStartDevice(GetLoopBeDeviceId());
     
                /*_midiInProc = new MIDIINPROC(MyMidiInProc);
                MIDIError ret = Midi.MIDI_InOpen(ref _midiInHandle, GetLoopBeDeviceId(), _midiInProc, IntPtr.Zero, MIDIFlags.MIDI_IO_STATUS);

                if (ret == MIDIError.MIDI_OK)
                {
                    // supply the device with 2 buffers
                   // AddSysExBuffer(_midiInHandle, 1024);
                  //  AddSysExBuffer(_midiInHandle, 1024);

                    ret = Midi.MIDI_InStart(_midiInHandle);
                    ret = Midi.MIDI_OutReset(_midiInHandle);
                }*/
                
            }
            catch(Exception vexp)
            {
                MessageBox.Show("Could not connect to Total Control");
            }
        }

        public void AddSysExBuffer(IntPtr handle, int size)
        {
            // prepare a empty midi header
            MIDI_HEADER header = new MIDI_HEADER(size);
            header.Prepare(true, handle);
            // If the header was perpared successfully.
            if (header.HeaderPtr != IntPtr.Zero)
            {
                // Add the buffer to the InputDevice.
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
                Debug.WriteLine(shortMsg.NoteString + " Note:" + shortMsg.Note + " " + shortMsg.Velocity );
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

        private static int GetLoopBeDeviceId()
        {
            
            var DeviceDescriptions = new List<string>(MidiInputDevice.GetDeviceDescriptions());
            int deviceId = 0;
            foreach (string deviceDescription in DeviceDescriptions)
            {
                // if (deviceDescription.Contains("LoopBe"))
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
            var outdevice = new MidiOutputDevice(device);

            MidiSysExMessage sysex = new MidiSysExMessage(false, outdevice.Device);
            sysex.CreateBuffer(new byte[] { 0xF0, 0x7E, 0x7F, 0x09, 0x01, 0xF7 });
            
            // send it

            _inDevice = new MidiInputDevice(device);

            
            //_inDevice.AutoPairController = true;

           // _inDevice.MessageFilter = MIDIMessageType.SystemCommon ;
            
            _inDevice.MessageReceived += new MidiMessageEventHandler(InDevice_MessageReceived);
            
            
            if (_inDevice.Open())
            {
                
                outdevice.Open();
                outdevice.Connect(outdevice.Device);
                        var msg = new MidiShortMessage();
                msg.StatusType = MIDIStatus.ControlChange;
                msg.Note = 48;
                msg.Channel = 0;
                msg.SongPosition = 0;
                msg.MessageType = MIDIMessageType.Channel;
                msg.Velocity = 255;
                if (!_inDevice.Start())
                    Console.WriteLine("Midi device could not be started! Error: " + _inDevice.LastErrorCode.ToString());
                System.Threading.Thread.Sleep(2000);
                outdevice.Send(msg);
            }
            else
                Console.WriteLine("Midi device could not be opened! Error: " + _inDevice.LastErrorCode.ToString());
        }
        private void InDevice_MessageReceived(object sender, MidiMessageEventArgs e)
        {
            
            
            Debug.WriteLine(e.ShortMessage?.Note + " "+ e.ShortMessage?.Velocity );
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
                return new DelegateCommand(()=> { BeatDetector.BeatRepeat = 1; });
            }
        }





    }
}
