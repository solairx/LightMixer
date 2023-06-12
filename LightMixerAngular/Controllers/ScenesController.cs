using LightMixer.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LightMixerAngular.Controllers
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

    public class SongHub : Hub<ISignalrDemoHub>
    {

        int x = 0;


        public void Hello()
        {
            Clients.Caller.DisplayMessage("Hello from the SignalrDemoHub!");
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        
    }

    public interface ISignalrDemoHub
    {
        Task DisplayMessage(string message);
    }
}