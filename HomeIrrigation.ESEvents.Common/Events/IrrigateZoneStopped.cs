using System;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
//using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HomeIrrigation.ESEvents.Common.Events
{
    public class IrrigateZoneStopped : Event
    {
        public IrrigateZoneStopped(Guid aggregateGuid, DateTimeOffset effectiveDateTime, IEventMetadata metadata) : base(aggregateGuid, effectiveDateTime, metadata)
        {
            AggregateGuid = aggregateGuid;
        }

        [JsonConstructor]
        private IrrigateZoneStopped(Guid aggregateGuid, string effectiveDateTime, string baseContentGuid, string description, EventMetadata metadata, int version) : this(aggregateGuid, DateTimeOffset.Parse(effectiveDateTime), metadata)
        {
            ExpectedVersion = version;
        }
    }
}
