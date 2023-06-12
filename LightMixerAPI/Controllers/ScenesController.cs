using LightMixer.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LightMixerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScenesController : ControllerBase
    {
        private readonly ILogger<ScenesController> _logger;
        private readonly SceneService sceneService;

        public ScenesController(ILogger<ScenesController> logger, SceneService sceneService)
        {
            _logger = logger;
            this.sceneService = sceneService;
        }

        [HttpGet]
        public IEnumerable<SceneViewModel> Get()
        {
            return sceneService.Scenes.Select(s => new SceneViewModel(s));
        }
        
    }

    [Route("[controller]")]
    [ApiController]
    public class SongHub : Hub
    {

        int x = 0;
        

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync(x++.ToString(), user, message);
        }
    }
}