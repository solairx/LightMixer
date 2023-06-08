using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LightMixer.Model.Service
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            /*var builder = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<SampleHostedService>();
                    services.AddMvcCore();
                    services.AddRouting();
                    services.AddSignalRCore();
                    
                })
            .Build()
            .RunAsync();

             //var builder = WebApplication.CreateBuilder(args);

             // Add services to the container.
             builder.Services.AddControllersWithViews();


             var app = builder.Build();

             // Configure the HTTP request pipeline.
             if (!app.Environment.IsDevelopment())
             {
                 app.UseExceptionHandler("/Home/Error");
                 // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                 app.UseHsts();
             }

             app.UseHttpsRedirection();
             app.UseStaticFiles();

             app.UseRouting();

             app.UseAuthorization();

             app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index2}/{id?}");

             app.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Lights}/{action=Index}/{id?}");

             app.Run();


             builder = services;//.AddMvcCore();
                                // services.AddSignalRCore();
             services.AddHostedService<LightMixerHubBackGroundService>();
             builder.AddFormatterMappings();

             builder.AddAuthorization();

             builder.AddViews();
             builder.AddRazorViewEngine();
             builder.AddCacheTagHelper();

             builder.AddDataAnnotations();

             services.AddLogging(opt =>
             {
                 opt.AddDebug();
             });*/
        }

       /* public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseEndpoint();
            app.UseEndpointRouting();
            app.UseMvc();
            
            app.UseRouting();
            app.UseEndpoint(endpoints =>
            {
                endpoints.MapHub<LightMixerHub>("/hub");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }*/
    }

    public class SampleHostedService : IHostedService
    {
        private readonly ILogger<SampleHostedService> logger;
        public SampleHostedService(ILogger<SampleHostedService> logger)
        {
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Hosted service starting");

            return Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    logger.LogInformation("Hosted service executing - {0}", DateTime.Now);
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
                    }
                    catch (OperationCanceledException) { }
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Hosted service stopping");
            return Task.CompletedTask;
        }
    }
}