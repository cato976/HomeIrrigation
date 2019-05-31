using HomeIrrigation.Common.EventBus;
using HomeIrrigation.ESEvents.Common.Events;
using HomeIrrigation.ESFramework.Common.Interfaces;

namespace HomeIrrigation.Api.Handlers
{
    public static class EventStoreHandlerRegistration
    {
        public static void RegisterEventHandler(IEventStore eventStore)
        {
            var rainHandlers = new EventStoreRainHandlers(eventStore);
            var eventBus = EventBus.Instance;
            eventBus.RemoveHandlers();
            eventBus.RegisterHandler<RainFell>(rainHandlers.Handler);
        }
    }
}
