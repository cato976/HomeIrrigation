using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HomeIrrigation.Sprinkler.Service
{
    public class DaemonService : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IOptions<DaemonConfig> _config;
        private HttpClient _httpClient;
        private ITimer _timer;

        public DaemonService(ILogger<DaemonService> logger, IOptions<DaemonConfig> config, ITimer timer, HttpClient httpClient)
        {
            _logger = logger;
            _config = config;
            _httpClient = httpClient;
            _timer = timer;
        }

#region IHostedService

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting daemon: " + _config.Value.DaemonName);
            // Start the timer
            var sp = new SprinklerService(_logger, _timer, _httpClient);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping daemon.");
            return Task.CompletedTask;
        }

#endregion IHostedService

#region IDisposable

        public void Dispose()
        {
            _logger.LogInformation("Disposing...");
        }

#endregion IDisposable
        
    }
}
