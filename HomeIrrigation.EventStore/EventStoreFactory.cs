using HomeIrrigation.ESFramework.Common.Interfaces;

namespace HomeIrrigation.EventStore
{
    public static class EventStoreFactory
    {
        public static IEventStore CreateEventStore()
        {
            return new EventStoreImplementation();
        }
    }
}
