using HomeIrrigation.ESEvents.Common.Events;
using HomeIrrigation.ESFramework.Common.Interfaces;

namespace HomeIrrigation.Sprinkler.Service.Handlers.Irrigation
{
    public class EventStoreIrrigationHandlers
    {
        public EventStoreIrrigationHandlers(IEventStore eventStore) { }

        public void Handler(IrrigateZoneStarted @event)
        {
            var zone = @event.AggregateGuid;
            var tenantId = @event.Metadata.TenantId;
        }

        public void Handler(IrrigateZoneStopped @event)
        {
            var zone = @event.AggregateGuid;
            var tenantId = @event.Metadata.TenantId;
        }
    }
}
