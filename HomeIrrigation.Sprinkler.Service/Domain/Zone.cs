using System;
using HomeIrrigation.Common.CommandBus;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.Sprinkler.Service.DataTransferObjects.Commands.Irrigation;
using Irrigation.Common.Commands;
using HomeIrrigation.Common;
using HomeIrrigation.ESEvents.Common.Events;
using HomeIrrigation.Sprinkler.Service.Enumerations;
using HomeIrrigation.EventStore.Exceptions;
using System.Diagnostics;

namespace HomeIrrigation.Sprinkler.Service.Domain
{
    public class Zone : Aggregate, IDisposable
    {
        public Zone()
        {
            Register<IrrigateZoneStarted>(OnIrrigateZoneStarted);
            Register<IrrigateZoneStopped>(OnIrrigateZoneStopped);
        }

        public Zone(Guid Id) : this()
        {
            AggregateGuid = Id;
        }

        public Zone(Guid zoneId, IEventStore eventStore) : this()
        {
            EventStore = eventStore;
            AggregateGuid = zoneId;
        }

        public ZoneState State { get; protected set; }
        private static IEventStore EventStore { get; set; }
        private IEventMetadata EventMetadata { get; set; }

        public void StartIrrigation(StartIrrigationCommand cmd, IEventMetadata eventMetadata)
        {
            ValidateCategory(eventMetadata.Category);
            EventMetadata = eventMetadata;
            var startIrrigationCommand = new StartIrrigation(cmd.TenantId, cmd.Zone, cmd.HowLongToIrrigate, EventStore);
            var commandBus = CommandBus.Instance;
            commandBus.Execute(startIrrigationCommand);
        }

        public void StopIrrigation(StopIrrigationCommand cmd, IEventMetadata eventMetadata)
        {
            ValidateCategory(eventMetadata.Category);
            EventMetadata = eventMetadata;
            var stopIrrigationCommand = new StopIrrigation(cmd.TenantId, cmd.Zone, EventStore);
            var commandBus = CommandBus.Instance;
            commandBus.Execute(stopIrrigationCommand);
        }

        public void IrrigateZone(IEventMetadata eventMetadata, IEventStore eventStore, double irrigationTime)
        {
            ValidateCategory(eventMetadata.Category);

            EventStore = eventStore;

            eventMetadata.PublishedDateTime = DateTimeOffset.UtcNow;
            ApplyEvent(new IrrigateZoneStarted(AggregateGuid, DateTimeOffset.UtcNow, eventMetadata, irrigationTime));

            // Send Event to Event Store
            var events = this.GetUncommittedEvents();
            EventSender.SendEvent(EventStore, new CompositeAggregateId(eventMetadata.TenantId, AggregateGuid, eventMetadata.Category), events);

            System.Threading.Thread.Sleep((int)irrigationTime * 1000 * 60);

            var stopIrrigationCommand = new StopIrrigation(eventMetadata.TenantId, AggregateGuid, EventStore);
            var commandBus = CommandBus.Instance;
            commandBus.Execute(stopIrrigationCommand);
        }

        public void StopZone(IEventMetadata eventMetadata, IEventStore eventStore, long originalEventNumber)
        {
            ValidateEventNumber(originalEventNumber);
            ValidateCategory(eventMetadata.Category);

            EventStore = eventStore;

            eventMetadata.PublishedDateTime = DateTimeOffset.UtcNow;
            ApplyEvent(new IrrigateZoneStopped(AggregateGuid, DateTimeOffset.UtcNow, eventMetadata), -4);

            // Send Event to Event Store
            var events = this.GetUncommittedEvents();
            try
            {
                EventSender.SendEvent(EventStore, new CompositeAggregateId(eventMetadata.TenantId, AggregateGuid, eventMetadata.Category), events);
            }
            catch(ConnectionFailure conn)
            {
                Trace.TraceError($"There was a connection error: {conn}");
            }
        }

        public void Apply(IrrigateZoneStarted e)
        {
            State = ZoneState.Running;
            DateTime scheduledTime = DateTime.Now.AddMinutes(e.HowLongToIrrigate);
            TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
            int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);
        }

        public void Apply(IrrigateZoneStopped e)
        {
            State = ZoneState.Off;
        }

        protected void ValidateEventNumber(long eventNumber)
        {
            if (((Aggregate)this).EventMetadata.EventNumber != eventNumber)
            {
                throw new ArgumentOutOfRangeException("event number", "Invalid event number specified: the event number is out of sync.");
            }
        }

        private static void ValidateCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentNullException("Invalid Category specified: cannot be null or empty.");
            }
            else if (!string.Equals(category.ToLowerInvariant(), "irrigation"))
            {
                throw new ArgumentNullException("Invalid Category specified: must be IRRIGATION.");
            }
        }

        private void OnIrrigateZoneStarted(IrrigateZoneStarted obj)
        {
            //throw new NotImplementedException();
        }

        private void OnIrrigateZoneStopped(IrrigateZoneStopped obj)
        {
            //throw new NotImplementedException();
        }

        private void StopZoneTimer(object sender)
        {

        }

        #region IDisposable

        public void Dispose()
        {
            StopZone(EventMetadata, EventStore, ((Aggregate)this).EventMetadata.EventNumber);
        }

        #endregion IDisposable
    }
}
