using System;
using System.Windows;
using LightMixer.Model.Service;
using Microsoft.Practices.Unity;
using LightMixer.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace LightMixer
{
    public class BootStrap
    {
        public static IUnityContainer UnityContainer
        {
            get;
            set;
        }

        public BootStrap()
        {
            new PhidGetService();
            UnityContainer = new UnityContainer();
            var beatDetector = new BeatDetector.BeatDetector();
            var sceneService = new SceneService(beatDetector);
            SharedEffectModel.BeatDetector = beatDetector;
            BootStrap.UnityContainer.RegisterInstance<BeatDetector.BeatDetector>(beatDetector);
            BootStrap.UnityContainer.RegisterInstance<SceneService>(sceneService);

            SharedEffectModel mn = new SharedEffectModel();
            UnityContainer.RegisterInstance<SharedEffectModel>(mn);
            UnityContainer.RegisterInstance<LightService.DmxServiceClient>(new LightService.DmxServiceClient());
            DmxModel model = new DmxModel();
            var dmxChaser = new DmxChaser();
            var sceneRenderedService = new SceneRenderedService(sceneService, beatDetector, dmxChaser);
            UnityContainer.RegisterInstance<DmxChaser>(dmxChaser);
            BootStrap.UnityContainer.RegisterInstance<SceneRenderedService>(sceneRenderedService);
            UnityContainer.RegisterInstance<DmxModel>(model);
            try
            {
                var host = CreateWebHostBuilder(new string[] { })
                    .Build()
                    .RunAsync();
            }
            catch (Exception vexp)
            {
                MessageBox.Show("Can't open webapi service for Listen, " + vexp.Message);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .ConfigureKestrel((context, options) =>
           {
               options.ListenAnyIP(8088, listenOptions =>
               {
                   listenOptions.Protocols = HttpProtocols.Http1;
               });
           })
           .UseStartup<Startup>();
    }
}
