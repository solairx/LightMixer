using HADotNet.Core;
using HADotNet.Core.Clients;
using Microsoft.Rest;
using System;
using System.Diagnostics;

namespace LightMixer.Model.Fixture
{
    public class HASSWRGWFixture : RgbFixture
    {
        private readonly string haEntity = "";
        private readonly bool isWhiteOnly = false;
        private readonly ServiceClient sv;

        public HASSWRGWFixture(string entitieName, bool isWhiteOnly)
        {
            ClientFactory.Initialize("http://192.168.1.12:8123/", "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiI4NTA2MGVmMzFhOGQ0Mjg0OWMxZWE0YmZmNjc2MWVlNyIsImlhdCI6MTYzOTUxODMzNCwiZXhwIjoxOTU0ODc4MzM0fQ.Oiy-yVGFQzbsaPS9k6bTOzAUu87UmI9lhUmbpuis0Tk");
            sv = ClientFactory.GetClient<ServiceClient>();
            haEntity = entitieName;
            isWhiteOnly = isWhiteOnly;
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
            if (!isDitry)
                return;
            isDitry = false;

            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                //  var resultingState = await sv.CallService("light", "turn_on", new { entity_id = "light.dimmer_plafond_ss" });
                if (this.RedValue + this.GreenValue + this.BlueValue < 20 && !isWhiteOnly)
                {
                    string json = "{\"entity_id\": \"" + haEntity + "\", \"rgbw_color\": [" + this.RedValue + ", " + this.GreenValue + ", " + this.BlueValue + ", 0]}";
                    //string json = "{\"entity_id\": \"light.chambre_maitre_rgb1_2\"" +
                    //        ", \"rgbw_color\": [0, 255, 0, 0]}";
                    var resultingState = sv.CallService("light", "turn_off", new { entity_id = haEntity });
                    //  resultingState.Wait();
                    //  Debug.WriteLine("Off " + sw.ElapsedMilliseconds + " " + haEntity );
                }
                else if (this.RedValue + this.GreenValue + this.BlueValue < 700 && isWhiteOnly)
                {
                    var resultingState = sv.CallService("light", "turn_off", new { entity_id = haEntity });
                    //resultingState.Wait();
                    // Debug.WriteLine("Off " + sw.ElapsedMilliseconds + " " + haEntity);
                }
                else if (isWhiteOnly)
                {
                    var resultingState = sv.CallService("light", "turn_on", new { entity_id = haEntity });
                    //resultingState.Wait();
                    // Debug.WriteLine("On  white" + sw.ElapsedMilliseconds + " " + haEntity);
                }
                else
                {
                    string json = "{\"entity_id\": \"" + haEntity + "\", \"rgbw_color\": [" + this.RedValue + ", " + this.GreenValue + ", " + this.BlueValue + ", 0]}";
                    //string json = "{\"entity_id\": \"light.chambre_maitre_rgb1_2\"" +
                    //        ", \"rgbw_color\": [0, 255, 0, 0]}";
                    var resultingState = sv.CallService("light", "turn_on", json);
                    //resultingState.Wait();
                    // Debug.WriteLine("On rgb" + sw.ElapsedMilliseconds + " " + haEntity);
                }
                //resultingState = await sv.CallService("light", "turn_on", new { entity_id = "light.chambre_maitre_rgb1_2", rgbw_color = "[255,0,0,0]" },);
                //System.Threading.Thread.Sleep(50);
                //resultingState = await sv.CallService("light", "turn_off", new { entity_id = "light.dimmer_plafond_ss" });
                //resultingState = await sv.CallService("light", "turn_off", new { entity_id = "light.chambre_maitre_rgb1_2" });
                //System.Threading.Thread.Sleep(700);
            }
            catch (Exception)
            {
            }
        }
    }
}