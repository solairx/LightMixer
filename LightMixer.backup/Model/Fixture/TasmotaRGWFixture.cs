using HADotNet.Core;
using HADotNet.Core.Clients;
using LightMixer.View;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

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

        public TasmotaRGWFixture(string ip)
        {
            client = HttpClientFactory.Create();
            client.Timeout = TimeSpan.FromSeconds(10);
            sv = ClientFactory.GetClient<ServiceClient>();
            this.ip = ip;

            Task.Run(HttpLoop);
        }

        public override int DmxLenght => 1;

        public override bool IsRenderOnDmx => false;

        public int? LastColorEnergy = null;

        public bool HasGreatLightEnergyDifference()
        {
            if (LastColorEnergy == null)
            {
                LastColorEnergy = RedValue + GreenValue + BlueValue;
                return true;
            }
            if (Math.Abs(LastColorEnergy.Value - (this.RedValue + this.GreenValue + this.BlueValue)) > 50)
            {
                LastColorEnergy = RedValue + GreenValue + BlueValue;
                return true;
            }
            LastColorEnergy = RedValue + GreenValue + BlueValue;
            return false;
        }

        public override byte?[] Render()
        {
            this.SetOn();
            return null;
        }

        public CancellationTokenSource cts = new CancellationTokenSource();
        private bool isHttpRunning = false;

        private void HttpLoop()
        {
            while (!MainWindow.IsDead)
            {
                try
                {
                    LastColorEnergy = RedValue + GreenValue + BlueValue;
                    var newQuery = "http://" + ip + "/cm?cmnd=Color%20" + ToHex(RedValue) + ToHex(GreenValue) + ToHex(BlueValue) + "0000&Dimmer%2088";
                    if (HasGreatLightEnergyDifference() && RedValue + GreenValue + BlueValue > 20)
                    {
                        Thread.Sleep(100);
                    }
                    if (lastQuery != newQuery)
                    {
                        lastQuery = newQuery;
                        cts = new CancellationTokenSource();
                        Debug.WriteLine(newQuery);
                        var task = Task.Run(() => client.GetAsync(newQuery));
                        var res = task.Result;
                        task.Wait();
                        ///     if (!task.Wait(3000))
                        {
                            Debug.WriteLine("TASMOTA FAILLLLLLLEDD");
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                }
            }
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

                /* if (lastQuery != newQuery && HasGreatLightEnergyDifference() && !isHttpRunning)
                 {
                     isHttpRunning = true;
                     Debug.WriteLine("Tasmota Query Cancel REquest" + cts.IsCancellationRequested);

                     cts = new CancellationTokenSource();
                     LastColorEnergy = RedValue + GreenValue + BlueValue;
                     lastQuery = newQuery;
                     Debug.WriteLine(newQuery);
                     client.GetAsync(newQuery, HttpCompletionOption.ResponseHeadersRead ,cts.Token)

                         .ContinueWith((a) => isHttpRunning = false) ;
                     cts.CancelAfter(400);
                 }*/
            }
            catch (Exception)
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