using System;
using Phidgets.Events;
using Phidgets;
using DmxLib;

namespace OpenDmx
{
    class Program
    {
        
        static bool switchOn = false;
        static OpenDMX dmx;
         public static void SetDmxValue(int channel, byte value)
        {
            if ((channel >= 513)
                ||
                (channel < 0))
                throw new InvalidOperationException("Channel number must be between 0 and " + 513);

            arr[channel] = value;

        }
         static byte[] arr = new byte[513];
        static void Main(string[] args)
        {
          
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            while (true)
            {
                int x = 0;
                for (x = 0; x < 513; x++)
                {
                    SetDmxValue(x,new byte());
                }
            }
            DmxController controller = new DmxController();
            controller.Start();
            Console.WriteLine("Ready and listening");
            Console.ReadLine();
            Console.WriteLine("pause");
            Console.ReadLine();
            bool quit = false;
            while (true)
            {
                int x;
                Random rnd = new Random();
                for (x = 1; x < 513; x++)
                {
                    DmxController.dmx.SetDmxValue(x, 225);
                    Console.WriteLine(x);
                    Console.ReadLine();
                }
            }
            controller.Stop();
        }
        public void old(){
            Console.WriteLine("Kit start");
            InterfaceKit kit = new InterfaceKit();
            
            kit.open();
            kit.waitForAttachment();
            kit.InputChange += new InputChangeEventHandler(kit_InputChange);

            Console.WriteLine("Attachted to switch");
            dmx = new OpenDMX();
            dmx.Start();
         /*   int x = 0;
            for (x = 1; x < OpenDmx.OpenDMX.CHANNEL_COUNT ; x++)
            {
                if (switchOn)
                    dmx.SetDmxValue(x, 64); // set DMX channel 1 to maximum value  
                else
                    dmx.SetDmxValue(x, 0); // set DMX channel 1 to maximum value  
            }*/
            /*dmx.SetDmxValue(1, 255); // set DMX channel 1 to maximum value
            dmx.SetDmxValue(2, 0); // set DMX channel 1 to maximum value
            dmx.SetDmxValue(3, 64); // set DMX channel 1 to maximum value
            dmx.SetDmxValue(4, 1); // set DMX channel 1 to maximum value
            dmx.SetDmxValue(5, 1); // set DMX channel 1 to maximum value
            dmx.SetDmxValue(6, 1); // set DMX channel 1 to maximum value
            dmx.SetDmxValue(7, 1); // set DMX channel 1 to maximum value*/
            Console.WriteLine("pause");
            Console.ReadLine();
            bool quit = false;
            while (true)
            {
                int x;
                Random rnd = new Random();
                for (x = 1; x < 513; x++)
                {
                    dmx.SetDmxValue(x, Convert.ToByte(rnd.Next(0,255)));
                }
            }
                Console.ReadLine();
            Console.WriteLine("stoping");
            dmx.Stop();
            

            Console.WriteLine("stoped");
        }

        static void kit_InputChange(object sender, InputChangeEventArgs e)
        {
            if (e.Index == 5)
                
                dmx.SwitchOn=  e.Value;

        }
    }



}

