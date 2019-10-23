using System;
using HomeIrrigation.Common.CommandBus;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.Sprinkler.Service.DataTransferObjects.Commands.Irrigation;
using Irrigation.Common.Commands;
using HomeIrrigation.Common;
using HomeIrrigation.ESEvents.Common.Events;

namespace HomeIrrigation.Sprinkler.Service.Domain
{
    public class Zone : Aggregate
    {
        public Zone(Guid Id)
        {
            AggregateGuid = Id;
        }

        public Zone(Guid zoneId, IEventStore eventStore)
        {
            EventStore = eventStore;
            AggregateGuid = zoneId;
        }

        private IEventStore EventStore { get; set; }

        public void StartIrrigation(StartIrrigationCommand cmd, IEventMetadata eventMetadata)
        {
            var startIrrigationCommand = new StartIrrigation(cmd.TenantId, cmd.Zone, cmd.HowLongToIrrigate, EventStore);
            var commandBus = CommandBus.Instance;
            commandBus.Execute(startIrrigationCommand);
        }

        public void IrrigateZone(IEventMetadata eventMetadata, IEventStore eventStore, int irrigationTime)
        {
            EventStore = eventStore;

            eventMetadata.PublishedDateTime = DateTimeOffset.UtcNow;
            ApplyEvent(new IrrigateZoneStarted(AggregateGuid, DateTimeOffset.UtcNow, eventMetadata));

            StartZone();

            // Send Event to Event Store
            var events = this.GetUncommittedEvents();
            EventSender.SendEvent(EventStore, new CompositeAggregateId(eventMetadata.TenantId, AggregateGuid, eventMetadata.Category), events);

            System.Threading.Thread.Sleep(irrigationTime * 1000 * 60);

            var stopIrrigationCommand = new StopIrrigation(eventMetadata.TenantId, AggregateGuid, EventStore);
            var commandBus = CommandBus.Instance;
            commandBus.Execute(stopIrrigationCommand);
        }

        private void StartZone()
        {
            
        }

        public void StopZone(IEventMetadata eventMetadata, IEventStore eventStore)
        {
            EventStore = eventStore;
            // Send Event to Event Store
            var events = this.GetUncommittedEvents();
            EventSender.SendEvent(EventStore, new CompositeAggregateId(eventMetadata.TenantId, AggregateGuid, eventMetadata.Category), events);
        }
    }
}
