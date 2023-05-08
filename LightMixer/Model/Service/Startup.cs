using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace LightMixer.Model.Service
{
   /* internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = services.AddMvcCore();
            services.AddSignalR();
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
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LightMixerHub>("/hub");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }*/
}