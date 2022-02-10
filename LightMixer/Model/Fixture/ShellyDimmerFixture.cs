using HADotNet.Core.Clients;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace LightMixer.Model.Fixture
{
    //http://192.168.1.21/light/0?turn=off

    public class ShellyDimmerFixture : RgbFixture
    {
        private readonly string haEntity = "";
        private readonly bool isWhiteOnly = false;
        private readonly ServiceClient sv;
        HttpClient client;
        Stopwatch minDelay;

        public ShellyDimmerFixture(string entitieName, bool isWhiteOnly)
        {
            client = new HttpClient();
            
           // ClientFactory.Initialize("http://192.168.1.12:8123/", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiI4NTA2MGVmMzFhOGQ0Mjg0OWMxZWE0YmZmNjc2MWVlNyIsImlhdCI6MTYzOTUxODMzNCwiZXhwIjoxOTU0ODc4MzM0fQ.Oiy-yVGFQzbsaPS9k6bTOzAUu87UmI9lhUmbpuis0Tk");
           // sv = ClientFactory.GetClient<ServiceClient>();
            haEntity = entitieName;
            isWhiteOnly = isWhiteOnly;
            minDelay = new Stopwatch();
            minDelay.Start();
        }

        public override int DmxLenght => 1;

        private bool isOnInternalState = true;

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

        public void SetOn()
        {

        //    if (!isDitry)
         //       return;
            isDitry = false;


            try
            {

                Stopwatch sw = new Stopwatch();
                sw.Start();
                //  var resultingState = await sv.CallService("light", "turn_on", new { entity_id = "light.dimmer_plafond_ss" });

                if (EnergyLevelIsLow && minDelay.ElapsedMilliseconds >100 && isOnInternalState)
                {
                    isOnInternalState = false;
                    client.GetAsync("http://192.168.1.17/light/0?turn=off");
                        //.Wait();
                }
                else if (!EnergyLevelIsLow && !isOnInternalState)
                {
                    isOnInternalState = true;
                    client.GetAsync("http://192.168.1.17/light/0?turn=on"); 
                       // .Wait();
                    minDelay = new Stopwatch();
                    minDelay.Start();
                }
                
            }
            catch (Exception v)
            {
            }


        }
    }
}
