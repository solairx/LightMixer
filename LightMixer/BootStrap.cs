using System;
using System.Windows;
using LightMixer.Model.Service;
using Microsoft.Practices.Unity;
using LightMixer.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using BeatDetector;
using System.Windows.Threading;
using LightMixer.View;

namespace LightMixer
{
    public class UiDispatcher : IDispatcher
    {
        private Dispatcher innerDispatcher;
        public UiDispatcher(Dispatcher dispatcher)
        {
            innerDispatcher = dispatcher;
        }
        public void Invoke(Action action)
        {
            innerDispatcher.Invoke(action);
        }
    }

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
    public class BootStrap
    {
        public static IUnityContainer UnityContainer
        {
            get;
            set;
        }

        public static IDispatcher Dispatcher;

        public BootStrap(IDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
            UnityContainer = new UnityContainer();
            UnityContainer.RegisterInstance<IDispatcher>(dispatcher);
            var virtualDjServer = new VirtualDjServer(()=>MainWindow.IsDead);
            UnityContainer.RegisterInstance<SharedEffectModel>( new SharedEffectModel(virtualDjServer));
            
            var dmxWrapper = new VComWrapper();
            dmxWrapper.initPro("com3");
            dmxWrapper.sendGetWidgetParametersRequest((ushort)0);
            UnityContainer.RegisterInstance(dmxWrapper);
            
            var beatDetector = new BeatDetector.BeatDetector();
            var sceneService = new SceneService(beatDetector);
            UnityContainer.RegisterInstance(new PhidGetService());
                      
            
            SharedEffectModel.BeatDetector = beatDetector;
            BootStrap.UnityContainer.RegisterInstance<BeatDetector.BeatDetector>(beatDetector);
            BootStrap.UnityContainer.RegisterInstance<SceneService>(sceneService);
                       
            UnityContainer.RegisterInstance<LightService.DmxServiceClient>(new LightService.DmxServiceClient());
            DmxModel model = new DmxModel();
            var dmxChaser = new DmxChaser(virtualDjServer);
            var sceneRenderedService = new SceneRenderedService(sceneService, dmxChaser, dmxWrapper, virtualDjServer);
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
                Debug.WriteLine(vexp.ToString());
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
