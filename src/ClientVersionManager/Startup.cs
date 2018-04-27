using ClientVersionManager.Routes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Linq;

namespace ClientVersionManager
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var basePath = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            //external setting for docker
            var configDirName = "docker-config";
            var dirPath = $"{basePath}{Path.DirectorySeparatorChar}{configDirName}";
            if (Directory.Exists(dirPath))
            {
                var skipDirectory = dirPath.Length;
                if (!dirPath.EndsWith("" + Path.DirectorySeparatorChar)) skipDirectory++;
                var fileNames = Directory.EnumerateFiles(dirPath, "*.json", SearchOption.AllDirectories)
                    .Select(f => f.Substring(skipDirectory));
                foreach (var fileName in fileNames)
                {
                    builder = builder.AddJsonFile($"{configDirName}{Path.DirectorySeparatorChar}{fileName}", optional: true, reloadOnChange: true);
                }
            }

            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute("default",
                   "{controller}/{action}/{id?}",
                   new { controller = "Home", action = "Index" },
                   new { controller = @"^(?!App).*$" }
               );

                routes.MapRoute("app",
                    "{appName:appName}/{action=Status}",
                    new { controller = "App" },
                    new { controller = @"App" }
                 );
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.Configure<RouteOptions>(options =>
                options.ConstraintMap.Add("appName", typeof(AppNameRouteConstraint)));
        }
    }
}