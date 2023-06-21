using LightMixer;
using LightMixer.Model;
using Microsoft.AspNetCore.SignalR;
using Unity;

namespace LightMixerAPI.Controllers
{
    public class TrackService
    {
        private IHubContext<SongHub> hubContext;
        private TrackInfo CurrentTrackInfo = new TrackInfo { trackInfo = "" };
        public TrackService(IHubContext<SongHub> _hubContext)
        {
            hubContext = _hubContext;
            SharedEffectModel = LightMixerBootStrap.UnityContainer.Resolve<SharedEffectModel>();
            Chaser = LightMixerBootStrap.UnityContainer.Resolve<DmxChaser>();
            SharedEffectModel.PropertyChanged += SharedEffectModel_PropertyChanged;
            Chaser.PropertyChanged += Chaser_PropertyChanged;
        }

        private void Chaser_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Chaser.TrackName))
            {
                CurrentTrackInfo.trackInfo = Chaser.TrackName;
                hubContext.Clients.All.SendAsync("TrackInfo", CurrentTrackInfo);
            }
        }

        private void SharedEffectModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SharedEffectModel.IsBeat))
            {
                CurrentTrackInfo.IsBeat = SharedEffectModel.IsBeat;
                hubContext.Clients.All.SendAsync("TrackInfo", CurrentTrackInfo);
            }

            
        }

        public SharedEffectModel SharedEffectModel { get; }
        public DmxChaser Chaser { get; }
    }
}