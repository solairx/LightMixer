using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeatDetector
{
    public class VirtualDjServer
    {
        private ConcurrentDictionary<Guid, VdjPipeServerInstance> InstanceList = new ConcurrentDictionary<Guid, VdjPipeServerInstance>();
        public VDJXmlParser vdjDataBase;

        public event VirtualDjServerEventHandler VirtualDjServerEvent;
        public delegate void VirtualDjServerEventHandler(VdjEvent vdjEvent);

        public VirtualDjServer()
        {
            vdjDataBase = new VDJXmlParser();
            StartNewInstance();
            StartMonitor();

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
                    while (true)
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
                        catch (Exception vexp1)
                        {

                        }
                    }
                }
                catch (Exception vexp)
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
