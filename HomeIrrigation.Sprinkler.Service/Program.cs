using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.EventStore;

namespace HomeIrrigation.Sprinkler.Service
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            config.AddEnvironmentVariables();
                            if (args != null)
                            {
                                config.AddCommandLine(args);
                            }
                        })
            .ConfigureServices((hostingContext, services) =>
                    {
                        services.AddOptions();
                        services.Configure<DaemonConfig>(hostingContext.Configuration.GetSection("Daemon"));

                        services.AddSingleton<IHostedService, DaemonService>();
                    })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
            });

            await builder.RunConsoleAsync();
        }
    }
}
