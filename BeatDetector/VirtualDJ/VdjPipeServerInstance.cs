using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace BeatDetector
{
    public class VdjPipeServerInstance
    {
        public event VirtualDjInstanceEventHandler VirtualDjInstanceEvent;
        public delegate void VirtualDjInstanceEventHandler(VdjEvent vdjEvent);
        private VDJXmlParser ingternalVdjDataBase;
        private Task internalTask;
        private DateTime? LastUpdate;
        private NamedPipeServerStream vdjPipe;
        internal void StartInstance(VDJXmlParser vdjDataBase)
        {
            IsWaitingForConnection = true;
            ingternalVdjDataBase = vdjDataBase;
            internalTask = StartVdjServer();
        }

        public bool IsWaitingForConnection { get; private set; }
        public bool IsInstanceDead()
        {
            if (LastUpdate != null && DateTime.Now.Subtract(LastUpdate.Value).Milliseconds > 100 && !IsWaitingForConnection)
            {
                vdjPipe.Close();
                return true;
            }
            return false;
        }

        private void ProcessVdjEvent(string messageLine)
        {
            Task.Factory.StartNew(() =>
            {
                messageLine = messageLine.Substring(0, messageLine.Length - 1);
                Dictionary<string, string> keyValuePairs = messageLine.Split(',')
                  .Select(value => value.Split('='))
                  .ToDictionary(pair => pair[0], pair => pair[1]);
                var vdjEvent = new VdjEvent();
                vdjEvent.FilePath = keyValuePairs["filePath"];
                vdjEvent.FileName = keyValuePairs["fileName"];
                vdjEvent.Elapsed = keyValuePairs["elapsed"];
                vdjEvent.BPM = keyValuePairs["bpm"];
                vdjEvent.CrossFader = GetDoubleFromMessage(keyValuePairs, "crossfader");
                vdjEvent.BeatNumber = GetDoubleFromMessage(keyValuePairs, "beatNum");
                vdjEvent.BeatBar16 = GetDoubleFromMessage(keyValuePairs, "beatBar16");
                vdjEvent.BeatBar = GetDoubleFromMessage(keyValuePairs, "beatBar");
                vdjEvent.BeatPos = GetDoubleFromMessage(keyValuePairs, "beatPos");
                vdjEvent.Volume = GetDoubleFromMessage(keyValuePairs, "volume");
                vdjEvent.Deck = GetIntFromMessage(keyValuePairs, "deck");
                var fullFileName = vdjEvent.FilePath + vdjEvent.FileName;
                VDJSong vdjSong = null;
                ingternalVdjDataBase.VDJDatabase.TryGetValue(fullFileName, out vdjSong);
                vdjEvent.VDJSong = vdjSong;
                if (VirtualDjInstanceEvent != null)
                    VirtualDjInstanceEvent(vdjEvent);
            });
        }

        private double GetDoubleFromMessage(Dictionary<string, string> source, string key)
        {
            string str = null;
            if (source.TryGetValue(key, out str))
            {
                try
                {
                    return Convert.ToDouble(str);
                }
                catch (Exception)
                {

                }
            }
            return 0;
        }

        private int GetIntFromMessage(Dictionary<string, string> source, string key)
        {
            string str = null;
            if (source.TryGetValue(key, out str))
            {
                try
                {
                    return Convert.ToInt32(str);
                }
                catch (Exception)
                {

                }
            }
            return 0;
        }

        private Task StartVdjServer()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {

                    PipeSecurity ps = new PipeSecurity();
                    ps.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
                    vdjPipe = new NamedPipeServerStream("virtualDJ", PipeDirection.InOut, 254);

                    vdjPipe.WaitForConnection();
                    IsWaitingForConnection = false;
                    byte[] buffer = new byte[5120];

                    var streamReader = new StreamReader(vdjPipe);
                    while (vdjPipe.IsConnected)
                    {
                        LastUpdate = DateTime.Now;
                        var upcommingCommand = streamReader.ReadLine();
                        LastUpdate = null;
                        if (!string.IsNullOrWhiteSpace(upcommingCommand))
                            ProcessVdjEvent(upcommingCommand);
                        Console.WriteLine(upcommingCommand);
                    }
                    IsWaitingForConnection = false;
                    vdjPipe.Close();
                }
                catch (Exception vexp)
                {
                    LastUpdate = DateTime.Now;
                    IsWaitingForConnection = false;
                }
            });
        }
    }

}
