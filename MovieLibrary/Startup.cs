
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieLibrary.Services;

namespace MovieLibrary
{
    internal class Startup
    {
        public IServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddFile("app.log");
            });


            services.AddTransient<IMainService, MainService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IRepository, JsonRepository>();

            return services.BuildServiceProvider();
        }
    }
}