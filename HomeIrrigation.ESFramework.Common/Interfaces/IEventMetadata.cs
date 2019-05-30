﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HomeIrrigation.ESFramework.Common.Interfaces
{
    public interface IEventMetadata
    {
        Guid TenantId { get; set; }
        string Category { get; set; }
        string CorrelationId { get; set; }
        Guid CausationId { get; set; }
        Guid AccountGuid { get; set; }
        DateTimeOffset PublishedDateTime { get; set; }
        [JsonIgnore]
        long EventNumber { get; set; }
        string EventId { get; set; }
        Dictionary<string, string> CustomMetadata { get; }
    }
}
