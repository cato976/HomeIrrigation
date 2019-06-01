using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.Api.DataTransferObjects.Commands.Rain;
using HomeIrrigation.Common.CommandBus;
using System;
using HomeIrrigation.Common;
using HomeIrrigation.ESEvents.Common.Events;

namespace HomeIrrigation.Api.Domain
{
    public class Rain : Aggregate
    {
        private Rain() { }

        private Rain(IEventMetadata eventMetadata, IEventStore eventStore, double inches)
        {
            ApplyEvent(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow, eventMetadata, inches));

            // Send Event to Event Store
            var events = this.GetUncommittedEvents();
            EventSender.SendEvent(eventStore, new CompositeAggregateId(eventMetadata.TenantId, AggregateGuid, eventMetadata.Category), events);
        }

        public static Rain RecordRainfall(IEventMetadata eventMetadata, IEventStore eventStore, RainFallCommand cmd)
        {
            var recordRainfallCommand = new RainFallCommand(cmd.Inches);
            var commandBus = CommandBus.Instance;
            commandBus.Execute(recordRainfallCommand);
            var rain = new Rain(eventMetadata, eventStore, cmd.Inches);
            return rain;
        }
    }
}
