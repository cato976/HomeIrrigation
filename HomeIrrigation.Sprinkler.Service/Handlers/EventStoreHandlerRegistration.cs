using HomeIrrigation.Common.EventBus;
using HomeIrrigation.ESEvents.Common.Events;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.Sprinkler.Service.Handlers.Irrigation;

namespace HomeIrrigation.Sprinkler.Service.Handlers
{
    public class EventStoreHandlerRegistration
    {
        public static void RegisterEventHandler(IEventStore eventStore)
        {
            var irrigationHandlers = new EventStoreIrrigationHandlers(eventStore);

            var eventBus = EventBus.Instance;
            eventBus.RemoveHandlers();
            eventBus.RegisterHandler<IrrigateZoneStarted>(irrigationHandlers.Handler);
            eventBus.RegisterHandler<IrrigateZoneStopped>(irrigationHandlers.Handler);
        }
    }
}
