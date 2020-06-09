using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http;

namespace LightMixer.Model.Service
{
    [ApiController]
    [Route("[controller]")]
    public class LightMixerController : ControllerBase

    {
        [HttpGet]
        public string Get()
        {
            return "Hello World!";
        }

        [HttpGet("{scene}/{zone}")]
        public IEnumerable<string> GetByName( string scene, string zone)
        {
            return new ObservableCollection<string>(BootStrap.UnityContainer.Resolve<DmxChaser>().LedEffectCollection.Select(o => o.Name)).ToArray();
        }

        [HttpGet("set/{scene}/{zone}/{effect}")]
        public void GetByName(string scene, string zone, string effect)
        {
            var effectSelected = BootStrap.UnityContainer.Resolve<DmxChaser>().LedEffectCollection.FirstOrDefault(o => o.Name == effect);
            if (effectSelected !=null)
            {
                BootStrap.UnityContainer.Resolve<DmxChaser>().CurrentLedEffect = effectSelected;
            }
        }


    }
}
