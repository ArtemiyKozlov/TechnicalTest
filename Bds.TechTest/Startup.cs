using System.Net;
using System.Net.Http;
using Bds.TechTest.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bds.TechTest
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            
            services.AddOptions();
            services.Configure<AppConfig>(Configuration);

            var config = Configuration.Get<AppConfig>();
            foreach (var engine in config.Engines)
            {
                services.AddHttpClient(engine.Name, c => {
                    c.BaseAddress = new System.Uri(engine.UrlBase);
                    c.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                }).ConfigurePrimaryHttpMessageHandler(configureHandler);
            }
        }

        private static HttpMessageHandler configureHandler(System.IServiceProvider messageHandler)
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.Brotli;
            }
            return handler;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endaponts =>
            {
                endaponts.MapRazorPages();
            });
        }
    }
}
