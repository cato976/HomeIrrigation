using HomeIrrigation.ESFramework.Common.Interfaces;
using System.Text.Json.Serialization;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HomeIrrigation.ESFramework.Common.Base
{
    public class EventMetadata : IEventMetadata
    {
        public EventMetadata()
        {

        }

        public EventMetadata(Guid tenantId, string category, Guid correlationId, Guid causationId,
            Guid accountGuid)
        {
            if (Guid.Empty == tenantId)
            {
                throw new ArgumentException(nameof(tenantId));
            }
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentNullException(nameof(category));
            }
            if (Guid.Empty == correlationId)
            {
                throw new ArgumentNullException(nameof(correlationId));
            }
            if (Guid.Empty == accountGuid)
            {
                throw new ArgumentException(nameof(accountGuid));
            }

            TenantId = tenantId;
            Category = category;
            CorrelationId = correlationId;
            CausationId = causationId;
            AccountGuid = accountGuid;
            PublishedDateTime = DateTime.UtcNow;
        }

        public Guid TenantId { get; set; }
        public string Category { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }
        public Guid AccountGuid { get; set; }
        public DateTimeOffset PublishedDateTime { get; set; }
        [JsonIgnore]
        public long EventNumber { get; set; }
        public string EventId { get; set; }
        public Dictionary<string, string> CustomMetadata { get; } = new Dictionary<string, string>();
    }
}
