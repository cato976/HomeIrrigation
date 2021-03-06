using System;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using Newtonsoft.Json;

namespace HomeIrrigation.ESEvents.Common.Events
{
    public class IrrigateZoneStared : Event
    {
        public IrrigateZoneStared(Guid aggregateGuid, DateTimeOffset effectiveDateTime, IEventMetadata metadata, int zone) : base(aggregateGuid, effectiveDateTime, metadata)
        {
            Zone = zone;
        }

        [JsonConstructor]
        private IrrigateZoneStared(Guid aggregateGuid, string effectiveDateTime, string baseContentGuid, string description, EventMetadata metadata, int zone, int version) : this(aggregateGuid, DateTimeOffset.Parse(effectiveDateTime), metadata, zone)
        {
            ExpectedVersion = version;
        }


        public int Zone {get; private set;}
    }
}
