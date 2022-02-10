using HADotNet.Core;
using HADotNet.Core.Clients;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace LightMixer.Model.Fixture
{

    public class TasmotaRGWFixture : RgbFixture
    {
        private readonly string ip = "";
        private HttpClient client;
        private readonly ServiceClient sv;
        private string lastQuery = "";
        private int SkipRate = 0;

        public override bool SupportAggresiveUpdate => false; 

        public TasmotaRGWFixture(string ip, bool isWhiteOnly)
        {
            client = new HttpClient();
            sv = ClientFactory.GetClient<ServiceClient>();
            this.ip = ip;
        }

        public override int DmxLenght => 1;

        public override bool IsRenderOnDmx => false;

        public override byte?[] Render()
        {
            this.SetOn();
            return null;
        }

        public void SetOn()
        {
            if (SkipRate < 3)
            {
                SkipRate++;
                // return;
            }
            SkipRate = 0;


            isDitry = false;


            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                var newQuery = "http://" + ip + "/cm?cmnd=Color%20" + ToHex(RedValue) + ToHex(GreenValue) + ToHex(BlueValue) + "0000&Dimmer%2088";
                if (lastQuery != newQuery)
                {
                    lastQuery = newQuery;
                    client.GetAsync(newQuery);
                }
            }
            catch (Exception v)
            {
            }


        }

        private string ToHex(byte value)
        {
            var hex = value.ToString("x");
            if (hex.Length > 1)
                return hex;
            else return "0" + hex;
        }
    }
}
