using HomeIrrigation.ESFramework.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace HomeIrrigation.ESFramework.Common.Base
{
    public abstract class Aggregate : IAggregate
    {
        internal readonly List<Event> events = new List<Event>();

        public Guid AggregateGuid { get; protected set; }

        private long version = -2;
        private EventMetadata eventMetadata;
        private readonly Dictionary<Type, Action<IEvent>> _handlers =
            new Dictionary<Type, Action<IEvent>>();
        public long Version { get { return version; } internal set { version = value; } }
        public EventMetadata EventMetadata { get { return eventMetadata; } internal set { eventMetadata = value; } }

        public IEnumerable<IEvent> GetUncommittedEvents()
        {
            return events;
        }

        public void MarkEventsAsCommitted()
        {
            events.Clear();
        }

        public void LoadStateFromHistory(IEnumerable<Event> history)
        {
            foreach (var evnt in history)
            {
                ApplyEvent(evnt, false);
            }
        }

        public void Replay(IEnumerable<IEvent> events)
        {
            if(events != null)
            {
                foreach(var @event in events)
                {
                    Version = @event.ExpectedVersion;
                    EventMetadata = @event.Metadata;
                    _handlers[@event.GetType()](@event);
                }
            }

        }

        protected internal void ApplyEvent(Event @event, long? streamExist = null)
        {
            ApplyEvent(@event, true, streamExist);
        }

        protected virtual void ApplyEvent(Event @event, bool isNew, long? streamExist = null)
        {
            this.AsDynamic().Apply(@event);
            if (isNew)
            {
                if(streamExist != null)
                {
                    @event.ExpectedVersion = (long)streamExist;
                }
                else
                {
                    @event.ExpectedVersion = Version;
                }
                events.Add(@event);
            }
            else
            {
                AggregateGuid = @event.AggregateGuid;
                EventMetadata = @event.Metadata;
                Version = @event.ExpectedVersion;
            }
        }

        protected void Register<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        {
            _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
        }
    }
}
