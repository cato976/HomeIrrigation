using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.EventStore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HomeIrrigation.Sprinkler.Service
{
    public class DaemonService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IOptions<DaemonConfig> _config;
        private IOptions<HttpClient> _httpClient;
        private Timer _timer;
        private static EventStoreImplementation EventStore { get; set; }
        private static Guid TenantId { get; set; }
        private SprinklerService sprinklerService;
        
        public DaemonService(ILogger<DaemonService> logger, IOptions<DaemonConfig> config, IOptions<Timer> timer, IOptions<HttpClient> httpClient)
        {
            _logger = logger;
            _config = config;
            _httpClient = httpClient;
            _timer = (Timer)timer.Value;
        }

        #region IHostedService

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting daemon: " + _config.Value.DaemonName);

            ConnectToEventStore(_config.Value);;
            // Start the timer
            sprinklerService = new SprinklerService(_logger, _timer, _httpClient.Value, EventStore, TenantId);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping daemon.");
            return Task.CompletedTask;
        }

        #endregion IHostedService

        private static void ConnectToEventStore(DaemonConfig config)
        {
            EventStore = (EventStoreImplementation)EventStoreFactory.CreateEventStore();
            TenantId = config.TenantId;
            string connectionString = "tcp://" + config.EventStoreUrl + ":" + config.EventStoreTcpPort;
            EventStore.Connect(connectionString, config.EventStoreUser,
                config.EventStorePassword,
                config.EventStoreCommonName,
                false, config.EventStoreReconnectionAttempts,
                config.EventStoreHeartbeatInterval, config.EventStoreHeartbeatTimeout);
        }
    }
}
