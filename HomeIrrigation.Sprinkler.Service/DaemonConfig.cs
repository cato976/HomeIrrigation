using System;

namespace HomeIrrigation.Sprinkler.Service
{
    public class DaemonConfig
    {
        public string DaemonName { get; set; }
        public Guid TenantId { get; set; }
        public string EventStoreUrl { get; set; }
        public int EventStoreTcpPort { get; set; }
        public string EventStoreUser { get; set; }
        public string EventStorePassword { get; set; }
        public string EventStoreCommonName { get; set; }
        public int EventStoreReconnectionAttempts { get; set; }
        public int EventStoreHeartbeatInterval { get; set; }
        public int EventStoreHeartbeatTimeout { get; set; }
    }
}
