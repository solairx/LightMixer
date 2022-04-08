using HADotNet.Core.Clients;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LightMixer.Model.Fixture
{
    //http://192.168.1.21/light/0?turn=off

    public class ShellyDimmerFixture : RgbFixture
    {


        HttpClient client;
        Stopwatch minDelay;
        public CancellationTokenSource cts = new CancellationTokenSource();

        public ShellyDimmerFixture(string ip)
        {

            client = HttpClientFactory.Create();
            client.Timeout = TimeSpan.FromSeconds(10);
            // ClientFactory.Initialize("http://192.168.1.12:8123/", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiI4NTA2MGVmMzFhOGQ0Mjg0OWMxZWE0YmZmNjc2MWVlNyIsImlhdCI6MTYzOTUxODMzNCwiZXhwIjoxOTU0ODc4MzM0fQ.Oiy-yVGFQzbsaPS9k6bTOzAUu87UmI9lhUmbpuis0Tk");
            // sv = ClientFactory.GetClient<ServiceClient>();
            minDelay = new Stopwatch();
            minDelay.Start();
            perf.Start();
        }
        public override bool SupportRGB => false;

        public override int DmxLenght => 1;

        public static  bool UseDarkMode
        {
            get;set;
        }

        private bool isOnInternalState = true;
        private double internalDimmerValue = 0;

        public override bool IsRenderOnDmx => false;

        public override byte?[] Render()
        {
            this.SetOn();
            return null;
        }

        private bool EnergyLevelIsLow
        {
            get
            {
                return this.RedValue + this.GreenValue + this.BlueValue < 100;
            }
        }
        Stopwatch perf = new Stopwatch();

        private string lastQuery = null;

        public void SetOn()
        {
            var newQuery = "";

            try
            {
                double dimmerValue = WhiteValue * 100 / 255;

                if (WhiteValue == 0  || UseDarkMode || this.currentEffect is AllOffEffect)
                {
                    newQuery = "http://192.168.1.252/light/0?turn=off&brightness=100";
                }
                else 
                {
                    isOnInternalState = true;
                    newQuery = "http://192.168.1.252/light/0?turn=on&brightness=" + dimmerValue;
                }

                if (newQuery != lastQuery && (minDelay.ElapsedMilliseconds > 150 || WhiteValue > 10))
                {
                    lastQuery = newQuery;
                    cts.Cancel();
                    cts = new CancellationTokenSource();
                    Debug.WriteLine("Shelly PreOff" + perf.ElapsedMilliseconds);
                    client.GetAsync(newQuery, cts.Token);
                    Debug.WriteLine("Shelly AfterOff" + perf.ElapsedMilliseconds);
                    minDelay = new Stopwatch();
                    minDelay.Start();
                }
                /*if (EnergyLevelIsLow && minDelay.ElapsedMilliseconds > 150 && isOnInternalState)
                {

                    isOnInternalState = false;
                    cts.Cancel();
                    cts = new CancellationTokenSource();
                    Debug.WriteLine("Shelly PreOff" + perf.ElapsedMilliseconds);
                    client.GetAsync("http://192.168.1.252/light/0?turn=off&brightness=100", cts.Token);
                    Debug.WriteLine("Shelly AfterOff" + perf.ElapsedMilliseconds);
                }
                else if (!EnergyLevelIsLow && !isOnInternalState)
                {

                    isOnInternalState = true;
                    CancellationTokenSource source = new CancellationTokenSource();
                    Debug.WriteLine("Shelly PreOn" + perf.ElapsedMilliseconds);
                    client.GetAsync("http://192.168.1.252/light/0?turn=on&brightness=100", cts.Token);
                    Debug.WriteLine("Shelly AfterOn" + perf.ElapsedMilliseconds);
                    minDelay = new Stopwatch();

                    minDelay.Start();
                }*/

            }
            catch (Exception v)
            {
            }


        }
    }
}
