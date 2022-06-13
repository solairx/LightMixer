using System;

namespace DmxClient
{
    class Program
    {
        static void Main(string[] args)
        {
            DmxService.DmxServiceClient client = new DmxService.DmxServiceClient();
            client.Open();
            while (true)
            {
                int x;
                Random rnd = new Random();
                for (x = 1; x < 513; x++)
                {
                    client.SetDmxChannel(x, 64);
                    Console.WriteLine(x);
                    Console.ReadLine();
                }
            }
        }
    }
}
