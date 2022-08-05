using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LightMixerBlazor.Data
{
    public class ZoneService
    {
        private readonly HttpClient httpClient;

        public ZoneService(LightHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<IEnumerable<string>> GetZone(string sceneName, string zoneName)
        {
            return httpClient.GetJsonAsync<IEnumerable<string>>("LightMixer/" + sceneName + "/" + zoneName + "/effectList");
        }

        public Task<IEnumerable<string>> GetZoneList(string sceneName)
        {
            return httpClient.GetJsonAsync<IEnumerable<string>>("LightMixer/" + sceneName + "/ZoneList");
        }

        public Task<string> GetZoneConfig(string sceneName)
        {
            return httpClient.GetStringAsync("LightMixer/" + sceneName + "/ZoneConfig");
        }

        public Task UpdateZone(string sceneName, string zoneName, string selectedEvent)
        {
            return httpClient.GetAsync("LightMixer/set/" + sceneName + "/" + zoneName + "/" + selectedEvent);
        }
    }
}