/*using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LightMixer.Model.Service
{
    public class LightMixerHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }

    public class LightMixerHubBackGroundService : IHostedService, IDisposable
    {
        public static IHubContext<LightMixerHub> HubContext;

        public LightMixerHubBackGroundService(IHubContext<LightMixerHub> hubContext)
        {
            HubContext = hubContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //TODO: your start logic, some timers, singletons, etc
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //TODO: your stop logic
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}*/