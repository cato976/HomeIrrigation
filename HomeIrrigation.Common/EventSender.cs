using log4net;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.ESFramework.Common.Base;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace HomeIrrigation.Common
{
    public static class EventSender
    {
        private static IEventStore EventStore;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EventSender));

        #region IEventSender

        public static bool SendEvent(IEventStore eventStore, CompositeAggregateId compositeId, IEnumerable<IEvent> events)
        {
            EventStore = eventStore;

            try
            {
                EventStore.SaveEvents(compositeId, events);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex.Message} in {Assembly.GetExecutingAssembly().GetName().Name}");
                throw;
            }
        }

        public static List<IEvent> GetAllEvents(IEventStore eventStore, CompositeAggregateId aggregateId)
        {
            EventStore = eventStore;

            try
            {
                return EventStore.GetAllEvents(aggregateId);
            }
            catch (Exception ex)
            {
                Logger.Error($"Exception: {ex.Message} in {Assembly.GetExecutingAssembly().GetName().Name}");
                throw;
            }
        }

        #endregion IEventSender

    }
}
