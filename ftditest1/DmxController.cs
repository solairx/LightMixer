using Phidgets;
using Phidgets.Events;
using System;
using System.ServiceModel;

namespace DmxLib
{
    public class DmxController
    {
        public static VComWrapper dmx;

        private byte[] buffer;

        //      public static  OpenDMX dmx;
        private static InterfaceKit kit;

        private ServiceHost host;

        public DmxController()
        {
        }

        public void Start()
        {
            try
            {
                kit = new InterfaceKit();

                kit.open("127.0.0.1", 5001);
                kit.waitForAttachment();
                kit.InputChange += new InputChangeEventHandler(kit_InputChange);
            }
            catch (Exception)
            {
            }
            dmx = new VComWrapper();
            dmx.initPro("com3");
            dmx.sendGetWidgetParametersRequest((ushort)0);

            ServiceHost host = new ServiceHost(typeof(DmxService), new Uri[2]
            {
                 new Uri("net.tcp://localhost:8523/DMX/DmxService"),
                 new Uri("http://localhost:8524/DMX/Metadata")
            });

            host.Open();
        }

        public void Stop()
        {
            //      dmx.Stop();
            dmx.detatchPro();
            kit.close();
            host.Close();
        }

        private DateTime LastChanged = DateTime.Now;
        private int lightIntensity = 0;

        private void kit_InputChange(object sender, InputChangeEventArgs e)
        {
            if (e.Index == 5)
            {
                if (DateTime.Now.Subtract(LastChanged).TotalSeconds < 2)
                {
                    if (lightIntensity == 255)
                    {
                        lightIntensity = 0;
                    }

                    lightIntensity += 32;

                    if (lightIntensity > 255)
                    {
                        lightIntensity = 255;
                    }
                }
                else if (lightIntensity != 0)
                {
                    lightIntensity = 0;
                }
                else
                {
                    lightIntensity = 255;
                }

                int x = 0;
                byte[] buffer = new byte[VComWrapper.CHANNEL_COUNT];
                for (x = 0; x < VComWrapper.CHANNEL_COUNT; x++)
                {
                    buffer[x] = Convert.ToByte(lightIntensity);
                }
                dmx.sendDMXPacketRequest(buffer);
            }

            LastChanged = DateTime.Now;
        }
    }
}