using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;


namespace LightMixer.Model.Service
{
    /*[ApiController]
    [Route("[controller]")]
    public class LightMixerController : Hub

    {
        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }

        [HttpGet("{scene}/ZoneConfig")]
        public Scene GetZonesConfig(string scene)
        {
            return BootStrap.UnityContainer.Resolve<SceneRenderedService>().sceneService.Scenes
                            .First(o => o.Name == scene);
        }

        [HttpGet("{scene}/ZoneList")]
        public IEnumerable<string> GetZonesList(string scene)
        {
            return BootStrap.UnityContainer.Resolve<SceneRenderedService>().sceneService.Scenes
                            .First(o => o.Name == scene).Zones.Select(o=> o.Name);
        }

        [HttpGet("{scene}/{zone}/effectList")]
        public IEnumerable<string> GetZoneFixtureEffectList(string scene, string zone)
        {
            return new ObservableCollection<string>(BootStrap.UnityContainer.Resolve<SceneRenderedService>().GetCurrentFixture<RGBLedFixtureCollection>(scene, zone)
                .SelectMany(o => o.EffectList)
                .Select(o => o.Name)
                .ToArray());
        }

        [HttpGet("set/{scene}/{zone}/{effect}")]
        public void SetZoneFixtureEffect(string scene, string zone, string effect)
        {
            var effectSelected = BootStrap.UnityContainer.Resolve<SceneRenderedService>().GetCurrentFixture<RGBLedFixtureCollection>(scene, zone)
                .SelectMany(o => o.EffectList)
                .FirstOrDefault(o => o.Name == effect);
            if (effectSelected != null)
            {
                BootStrap.UnityContainer.Resolve<SceneRenderedService>().SetCurrentEffect<RGBLedFixtureCollection>(scene, zone, effectSelected);
            }
        }

        public async Task UpdateZoneFixtureEffect(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }*/

    


}