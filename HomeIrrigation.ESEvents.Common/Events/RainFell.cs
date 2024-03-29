using System;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
//using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HomeIrrigation.ESEvents.Common.Events
{
    public class RainFell : Event
    {
        public RainFell(Guid aggregateGuid, DateTimeOffset effectiveDateTime, IEventMetadata metadata, double inches) : base(aggregateGuid, effectiveDateTime, metadata)
        {
            Inches = inches;
        }

        [JsonConstructor]
        private RainFell(Guid aggregateGuid, string effectiveDateTime, string baseContentGuid, string description, EventMetadata metadata, double inches, int version) : this(aggregateGuid, DateTimeOffset.Parse(effectiveDateTime), metadata, inches)
        {
            ExpectedVersion = version;
        }

        [JsonInclude]
        public double Inches { get; private set; }
    }
}
