using BeatDetector;
using LightMixer.Model;
using LightMixer.Model.Service;
using LightMixer.View;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Practices.Unity;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

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
            LightMixerBootStrap bootstrap = new LightMixerBootStrap(dispatcher);

           
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