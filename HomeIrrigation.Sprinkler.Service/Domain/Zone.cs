using System;
using HomeIrrigation.Common;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;

namespace HomeIrrigation.Sprinkler.Service.Domain
{
    public class Zone : Aggregate
    {
        public Zone(IEventStore eventStore, Guid zoneId)
        {
            EventStore = eventStore;
            AggregateGuid = zoneId;
        }

        private IEventStore EventStore { get; set; }

        public void StartIrrigation(double howLongToIrrigate, IEventMetadata eventMetadata)
        {
            var events = this.GetUncommittedEvents();
            EventSender.SendEvent(EventStore, new CompositeAggregateId(eventMetadata.TenantId, AggregateGuid, eventMetadata.Category), events);
        }
    }
}
