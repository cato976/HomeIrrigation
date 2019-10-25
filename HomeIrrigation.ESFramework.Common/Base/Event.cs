﻿using HomeIrrigation.ESFramework.Common.Interfaces;
using System;

namespace HomeIrrigation.ESFramework.Common.Base
{
    public class Event : IEvent
    {
        public Event(Guid aggregateGuid, DateTimeOffset effectiveDateTime, IEventMetadata metadata)
        {
            if (effectiveDateTime == DateTime.MinValue)
            {
                throw new ArgumentException(nameof(effectiveDateTime));
            }
            AggregateGuid = aggregateGuid;
            EffectiveDateTime = effectiveDateTime;
            Metadata = metadata as EventMetadata ?? throw new ArgumentNullException(nameof(metadata));
        }

        public Guid AggregateGuid { get; set; /*protected set;*/ }
        public EventMetadata Metadata { get; }
        public DateTimeOffset EffectiveDateTime { get; }
        public long ExpectedVersion { get; set; }
    }
}
