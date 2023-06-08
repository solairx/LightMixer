using BeatDetector;
using LightMixer.Model;
using LightMixer.Model.Service;
using LightMixer.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Windows;
using Unity;

namespace LightMixer
{
    

    public class ServiceDispatcher : IDispatcher
    {
        public ServiceDispatcher()
        {
        }

        public void Invoke(Action action)
        {
            action.Invoke();
        }
    }

    public interface IDispatcher
    {
        void Invoke(Action action);
    }

    public class LightMixerBootStrap
    {
        public static IUnityContainer UnityContainer
        {
            get;
            set;
        }

        public static IDispatcher Dispatcher;

        public static bool IsDead { get; set; } = false;

        public LightMixerBootStrap(IDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            UnityContainer = new UnityContainer();
            UnityContainer.RegisterInstance<IDispatcher>(dispatcher);
            var virtualDjServer = new VirtualDjServer(() => LightMixerBootStrap.IsDead);
            UnityContainer.RegisterInstance<SharedEffectModel>(new SharedEffectModel(virtualDjServer));

            var dmxWrapper = new VComWrapper();
            dmxWrapper.initPro("com3");
            dmxWrapper.sendGetWidgetParametersRequest((ushort)0);
            UnityContainer.RegisterInstance(dmxWrapper);

            var beatDetector = new BeatDetector.BeatDetector();
            var sceneService = new SceneService(beatDetector);
           // UnityContainer.RegisterInstance(new PhidGetService());

            SharedEffectModel.BeatDetector = beatDetector;
            LightMixerBootStrap.UnityContainer.RegisterInstance<BeatDetector.BeatDetector>(beatDetector);
            LightMixerBootStrap.UnityContainer.RegisterInstance<SceneService>(sceneService);

            DmxModel model = new DmxModel();
            var dmxChaser = new DmxChaser(virtualDjServer);
            var sceneRenderedService = new SceneRenderedService(sceneService, dmxChaser, dmxWrapper, virtualDjServer);
            UnityContainer.RegisterInstance<DmxChaser>(dmxChaser);
            LightMixerBootStrap.UnityContainer.RegisterInstance<SceneRenderedService>(sceneRenderedService);
            UnityContainer.RegisterInstance<DmxModel>(model);
            /*try
            {
                var host = CreateWebHostBuilder(new string[] { })
                    .Build()
                    .RunAsync();

    
            }
            catch (Exception vexp)
            {
                Debug.WriteLine(vexp.ToString());
                MessageBox.Show("Can't open webapi service for Listen, " + vexp.Message);
            }*/
        }

       /* public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
           .ConfigureKestrel((context, options) =>
           {
               options.ListenAnyIP(8088, listenOptions =>
               {
                   listenOptions.Protocols = HttpProtocols.Http1;
               });
           })
           .UseStartup<Startup>();*/
    }
}