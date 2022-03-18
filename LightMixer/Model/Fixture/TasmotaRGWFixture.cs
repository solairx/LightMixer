using HADotNet.Core;
using HADotNet.Core.Clients;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

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
            client = HttpClientFactory.Create(); 
            client.Timeout = TimeSpan.FromSeconds(10);
            sv = ClientFactory.GetClient<ServiceClient>();
            this.ip = ip;
        }

        public override int DmxLenght => 1;

        public override bool IsRenderOnDmx => false;

        public int? LastColorEnergy = null;

        public bool HasGreatLightEnergyDifference ()
        {
            if (LastColorEnergy == null)
            {
                return true;
            }
            if (Math.Abs(LastColorEnergy.Value - (this.RedValue + this.GreenValue + this.BlueValue)) >50)
            {
                return true;
            }
            return false;
        }

        public override byte?[] Render()
        {
            this.SetOn();
            return null;
        }
        public CancellationTokenSource cts = new CancellationTokenSource();

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
                if (lastQuery != newQuery && HasGreatLightEnergyDifference())
                {
                    cts.Cancel();
                    cts = new CancellationTokenSource();
                    LastColorEnergy = RedValue + GreenValue + BlueValue;
                    lastQuery = newQuery;
                    client.GetAsync(newQuery, cts.Token);
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
