using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
//using System.Web.Http;

namespace LightMixer.Model.Service
{
    class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder =  services.AddMvcCore();
            builder.AddJsonFormatters();
            // Default framework order
            builder.AddFormatterMappings();
            
            builder.AddAuthorization();
            /*builder.AddApiExplorer();
            
            builder.AddViews();
            builder.AddRazorViewEngine();
            builder.AddCacheTagHelper();

            // +1 order
            builder.AddDataAnnotations(); // +1 order*/
            services.AddLogging(opt =>
            {
                opt.AddDebug();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            app.UseMvcWithDefaultRoute();
        }
    }
}
