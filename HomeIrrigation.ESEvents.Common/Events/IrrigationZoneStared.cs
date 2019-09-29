using System;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using Newtonsoft.Json;

namespace HomeIrrigation.ESEvents.Common.Events
{
    public class IrrigationZoneStared : Event
    {
        public IrrigationZoneStared(Guid aggregateGuid, DateTimeOffset effectiveDateTime, IEventMetadata metadata, int zone) : base(aggregateGuid, effectiveDateTime, metadata)
        {
            Zone = zone;
        }

        [JsonConstructor]
        private IrrigationZoneStared(Guid aggregateGuid, string effectiveDateTime, string baseContentGuid, string description, EventMetadata metadata, int zone, int version) : this(aggregateGuid, DateTimeOffset.Parse(effectiveDateTime), metadata, zone)
        {
            Version = version;
        }


        public int Zone {get; private set;}
    }
}
