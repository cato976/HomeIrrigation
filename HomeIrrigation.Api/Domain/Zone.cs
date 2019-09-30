using System;
using HomeIrrigation.Api.DataTransferObjects.Commands;
using HomeIrrigation.Common;
using HomeIrrigation.ESEvents.Common.Events;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;

namespace HomeIrrigation.Api.Domain
{
    public class Zone : Aggregate
    {
        public Zone()
        {
        }

        public IEventStore EventStore { get; set; }

        public void IrrigateZone(IEventMetadata eventMetadata, IEventStore eventStore, IrrigateZoneCommand cmd)
        {
            EventStore = eventStore;

            eventMetadata.PublishedDateTime = DateTimeOffset.UtcNow;
            ApplyEvent(new IrrigateZoneStared(cmd.Id, DateTimeOffset.UtcNow, eventMetadata, cmd.Zone));

            // Send Event to Event Store
            var events = this.GetUncommittedEvents();
            EventSender.SendEvent(EventStore, new CompositeAggregateId(eventMetadata.TenantId, AggregateGuid, eventMetadata.Category), events);
        }
    }
}
