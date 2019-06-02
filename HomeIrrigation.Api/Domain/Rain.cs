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

        public double Inches { get; private set; }

        public static Rain RecordRainfall(IEventMetadata eventMetadata, IEventStore eventStore, RainFallCommand cmd)
        {
            var recordRainfallCommand = new RainFallCommand(cmd.Inches);
            var commandBus = CommandBus.Instance;
            commandBus.Execute(recordRainfallCommand);
            var rain = new Rain(eventMetadata, eventStore, cmd.Inches);
            return rain;
        }

        public static double GetWeeklyRainfall(IEventStore eventStore, CompositeAggregateId aggregateId)
        {
            double rainfall = 0;
            var events = EventSender.GetAllEvents(eventStore, aggregateId);
            foreach (var item in events)
            {
                if (item.GetType() == typeof(RainFell))
                {
                    var fell = item as RainFell;
                    if (string.Equals(fell.Metadata.Category.ToLower(), "rain") && item.EffectiveDateTime > DateTimeOffset.UtcNow.AddDays(-7))
                    {
                        rainfall += ((RainFell)item).Inches;
                    }
                }
            }
            return rainfall;
        }
    }
}
