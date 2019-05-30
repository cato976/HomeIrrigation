using HomeIrrigation.ESFramework.Common.Base;
using System;

namespace HomeIrrigation.ESFramework.Common.Interfaces
{
    public interface IEvent
    {
        Guid AggregateGuid { get;  }
        EventMetadata Metadata { get; }
        DateTimeOffset EffectiveDateTime { get; }
        int Version { get; }
    }
}
