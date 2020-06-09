using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LightMixerBlazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).ConfigureServices(ConfigureService).Build().Run();
        }

        private static void ConfigureService(HostBuilderContext arg1, IServiceCollection services)
        {
            
            if (!services.Any(x => x.ServiceType == typeof(LightHttpClient)))
            {
                // Setup HttpClient for server side in a client side compatible fashion
                services.AddSingleton<LightHttpClient>(s =>
                {
                    // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                    
                    return new LightHttpClient
                    {
                        BaseAddress = new Uri("http://127.0.0.1:8088") // LightMixer URL
                    };
                });
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
