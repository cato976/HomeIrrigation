using HomeIrrigation.ESFramework.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HomeIrrigation.ESFramework.Common.Base
{
    public class Repository<T> : IRepository<T> where T : Aggregate, new()
    {
        public Repository(IEventStore storage)
        {
            Storage = storage;
        }

        private readonly IEventStore Storage;

        #region IRepository

        public T GetById(CompositeAggregateId aggregateId)
        {
            var obj = new T();
            try
            {
                var eventInterfaces = Storage.GetAllEvents(aggregateId);
                //obj.Replay(e);
                List<Event> events = new List<Event>();
                foreach(var item in eventInterfaces)
                {
                    events.Add((Event)item);
                }

                obj.LoadStateFromHistory(events);
            }
            catch(Exception ex)
            {
                Trace.TraceError($"Error: {ex}");
            }

            return obj;
        }

        #endregion IRepository
    }
}
