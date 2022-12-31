using System;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
//using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HomeIrrigation.ESEvents.Common.Events
{
    public class IrrigateZoneStarted : Event
    {
        public IrrigateZoneStarted(Guid aggregateGuid, DateTimeOffset effectiveDateTime, IEventMetadata metadata, double howLongToIrrigate) : base(aggregateGuid, effectiveDateTime, metadata)
        {
            AggregateGuid = aggregateGuid;
            HowLongToIrrigate = howLongToIrrigate;
        }

        [JsonConstructor]
        private IrrigateZoneStarted(Guid aggregateGuid, string effectiveDateTime, string baseContentGuid, string description, EventMetadata metadata, long howLongToIrrigate, int version) : this(aggregateGuid, DateTimeOffset.Parse(effectiveDateTime), metadata, howLongToIrrigate)
        {
            ExpectedVersion = version;
        }

        public double HowLongToIrrigate { get; private set; }
    }
}
