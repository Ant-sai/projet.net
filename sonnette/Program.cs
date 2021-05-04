using System;
using System.Device.Gpio;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using sonnette.Configuration;

namespace sonnette
{
    class Program
    {
        private static ServiceCollection Services { get; set; }
        static void Main(string[] args)
        {
            Services = new ServiceCollection();
            configureServices(Services);
            IServiceProvider serviceProvider = Services.BuildServiceProvider();

            try
            {
                serviceProvider.GetService<App>().Run();
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
    static private void configureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            services.AddSingleton(configuration);

            services.Configure<AppConfig>(configuration.GetSection("AppConfiguration"));

            // Add app
            services.AddTransient<App>();

        }
    
    }
}
