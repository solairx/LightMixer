using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeatDetector
{
    public class VirtualDjServer
    {
        private ConcurrentDictionary<Guid, VdjPipeServerInstance> InstanceList = new ConcurrentDictionary<Guid, VdjPipeServerInstance>();
        public VDJXmlParser vdjDataBase;
        private readonly Func<bool> isDeadGetter;

        public event VirtualDjServerEventHandler VirtualDjServerEvent;

        public delegate void VirtualDjServerEventHandler(VdjEvent vdjEvent);

        public event OS2lServerHandler OS2lServerEvent;

        public delegate void OS2lServerHandler(OS2lEvent os2lEvent);

        public static Func<bool> IsDead = () => false;

        public VirtualDjServer(Func<bool> IsDeadGetter)
        {
            isDeadGetter = IsDeadGetter;
            IsDead = IsDeadGetter;
            vdjDataBase = new VDJXmlParser();
            StartNewInstance();
            StartMonitor();
            StartOS2l();
        }

        private void StartOS2l()
        {
            RegisterServiceToBonjour();
            Task.Run(() => StartOS2lListener());
        }

        private void StartOS2lListener()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 4444;
                server = new TcpListener(port);
                try
                {
                    server.Start();
                }
                catch (SocketException)
                {
                    MessageBox.Show("Can't start listener on port 4444 for os2l");
                }
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (!isDeadGetter())
                {
                    TcpClient client = server.AcceptTcpClient();

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // sample :  {"evt":"beat","change":false,"pos":226,"bpm":134,"strength":0.7}
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);
                        data = data.ToLower();
                        ParseOS2lMsg(data);
                        data.Replace("{", "");
                    }
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Debug.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }

        private void ParseOS2lMsg(string data)
        {
            try
            {
                data = data.ToLower();

                var jsonstr = (JObject)JsonConvert.DeserializeObject(data);

                var newEvent = new OS2lEvent();
                foreach (var d in jsonstr.Children())
                {
                    var key = d.Path;
                    var value = ((JValue)((JProperty)d).Value).Value;
                    if (key == "pos")
                    {
                        newEvent.BeatPos = (Int64)value;
                    }

                    if (key == "bpm" && value is double)
                    {
                        newEvent.Bpm = (double)value;
                    }
                    else if (key == "bpm" && value is int)
                    {
                        newEvent.Bpm = (int)value;
                    }
                }
                if (newEvent.Bpm != 0 && newEvent.BeatPos != 0)
                {
                    newEvent.Elapsed = newEvent.BeatPos * (60 / newEvent.Bpm);
                }
                OS2lServerEvent?.Invoke(newEvent);
            }
            catch (Exception)
            {
            }
        }

        private void RegisterServiceToBonjour()
        {
            var svc = new ArkaneSystems.Arkane.Zeroconf.RegisterService();
            svc.Name = "os2l";
            svc.RegType = "_os2l._tcp.";
            svc.Port = 4444;
            svc.Register();
        }

        private void StartNewInstance()
        {
            var serverInstance = new VdjPipeServerInstance();
            InstanceList.TryAdd(Guid.NewGuid(), serverInstance);
            serverInstance.StartInstance(vdjDataBase);
            serverInstance.VirtualDjInstanceEvent += ServerInstance_VirtualDjInstanceEvent;
        }

        private Task StartMonitor()
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    while (true && !this.isDeadGetter())
                    {
                        try
                        {
                            var deadInstance = InstanceList.Where(inst => inst.Value.IsInstanceDead());

                            foreach (var instance in deadInstance)
                            {
                                VdjPipeServerInstance removedInstance;
                                InstanceList.TryRemove(instance.Key, out removedInstance);
                                // instance.VirtualDjInstanceEvent -= ServerInstance_VirtualDjInstanceEvent;
                                StartNewInstance();
                            }
                            if (!InstanceList.Any(inst => inst.Value.IsWaitingForConnection))
                            {
                                StartNewInstance();
                            }

                            Thread.Sleep(100);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private void ServerInstance_VirtualDjInstanceEvent(VdjEvent vdjEvent)
        {
            if (VirtualDjServerEvent != null)
                VirtualDjServerEvent(vdjEvent);
        }
    }
}