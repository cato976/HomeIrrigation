using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.ESEvents.Common.Events;

namespace HomeIrrigation.Api.Handlers
{
    public class EventStoreRainHandlers
    {
        public EventStoreRainHandlers(IEventStore eventStore)
        {
        }

        public void Handler(RainFell @event)
        {
        }
    }
}
