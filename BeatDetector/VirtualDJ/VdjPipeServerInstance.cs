using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private static List<string> lastLog = new List<string> ();

        private void ProcessVdjEvent(string messageLine)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    messageLine = messageLine.Substring(0, messageLine.Length - 1);
                    //                Debug.WriteLine(messageLine);
                    Dictionary<string, string> keyValuePairs = messageLine.Split('*')
                      .Select(value => value.Split(':'))
                      .ToDictionary(pair => pair[0], pair => pair[1]);
                    var vdjEvent = new VdjEvent();
                    vdjEvent.FilePath = keyValuePairs["filePath"];
                    vdjEvent.FileName = keyValuePairs["fileName"];
                    var candidateLogLine = vdjEvent.FilePath + vdjEvent.FileName;
                    if (!lastLog.Contains(candidateLogLine) && candidateLogLine.Length > 5)
                    {
                        lock (VirtualDjServer.LogWriter)
                        {
                            if (!lastLog.Contains(candidateLogLine) && candidateLogLine.Length > 5)
                            {
                                try
                                {
                                    var LogWriter = new StreamWriter(@"d:\virtualdj\lightmixer.log", true);
                                    LogWriter.WriteLine(@"D:" + candidateLogLine);
                                    LogWriter.Close();
                                    lastLog.Add(candidateLogLine);
                                }
                                catch (Exception vexp)
                                {

                                }
                            }
                        }
                    }
                    vdjEvent.Elapsed = keyValuePairs["elapsed"];
                    vdjEvent.BPM = keyValuePairs["bpm"];
                    vdjEvent.CrossFader = GetDoubleFromMessage(keyValuePairs, "crossfader");
                    //vdjEvent.BeatNumber = GetDoubleFromMessage(keyValuePairs, "beatNum");
                    //vdjEvent.BeatBar16 = GetDoubleFromMessage(keyValuePairs, "beatBar16");
                    //vdjEvent.BeatBar = GetDoubleFromMessage(keyValuePairs, "beatBar");
                    vdjEvent.BeatGrid = GetDoubleFromMessage(keyValuePairs, "beatGrid");
                    vdjEvent.BeatPos = GetDoubleFromMessage(keyValuePairs, "beatPos");
                    vdjEvent.Volume = GetDoubleFromMessage(keyValuePairs, "volume");
                    vdjEvent.Deck = GetIntFromMessage(keyValuePairs, "deck");
                    var fullFileName = vdjEvent.FilePath + vdjEvent.FileName;
                    VDJSong vdjSong = null;
                    ingternalVdjDataBase.VDJDatabase.TryGetValue(fullFileName, out vdjSong);
                    vdjEvent.VDJSong = vdjSong;
                    if (vdjSong !=null && !vdjSong.ZplaneLoad)
                    {
                        vdjSong.LoadZplace();
                    }
                    if (vdjSong != null && !vdjSong.MLSongModel.MusicMLLoad)
                    {
                        vdjSong.MLSongModel.LoadMusicML();
                    }
                    if (vdjSong != null && !vdjSong.AutomationLoad)
                    {
                        vdjSong.LoadAutomation();
                    }

                    
                    VirtualDjInstanceEvent?.Invoke(vdjEvent);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Message received from Virtual DJ is invalid " + messageLine);
                }
            });
        }

        private double GetDoubleFromMessage(Dictionary<string, string> source, string key)
        {
            string str;
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
                    var sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                    NTAccount account = (NTAccount)sid.Translate(typeof(NTAccount));
                    ps.SetAccessRule(new PipeAccessRule(account, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
                    //NamedPipeServerStream server = new NamedPipeServerStream("TestPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.None, 512, 512, ps)

                    //PipeSecurity ps = new PipeSecurity();
                    //ps.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null), PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow));
                    //vdjPipe = new NamedPipeServerStream("virtualDJ", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.None, 512, 512, ps) ;//
                    vdjPipe = new NamedPipeServerStream("virtualDJ", PipeDirection.InOut, 254);

                    vdjPipe.WaitForConnection();
                    IsWaitingForConnection = false;
                    byte[] buffer = new byte[5120];

                    var streamReader = new StreamReader(vdjPipe);
                    while (vdjPipe.IsConnected && !VirtualDjServer.IsDead.Invoke())
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
                catch (Exception)
                {
                    LastUpdate = DateTime.Now;
                    IsWaitingForConnection = false;
                }
            });
        }
    }
}