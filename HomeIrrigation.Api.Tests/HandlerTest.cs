using System;
using HomeIrrigation.Api.Handlers;
using HomeIrrigation.Common.EventBus;
using Moq;
using NUnit.Framework;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESEvents.Common.Events;

namespace HomeIrrigation.Api.Test
{
    public class HandlerTest
    {
        Mock<IEventStore> moqEventStore;
        IEventMetadata eventMetadata;

        [SetUp]
        public void Setup()
        {
            moqEventStore = new Mock<IEventStore>();
            eventMetadata = new EventMetadata(Guid.NewGuid(), "TestCategory", "TestCorrelationId", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
        }

        [Test]
        public void Should_Handle_Rain_Fell_Event()
        {
            PassEventToEventBus(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow,
                eventMetadata, 56));
        }

        private void PassEventToEventBus(IEvent handledEvent)
        {
            EventStoreHandlerRegistration.RegisterEventHandler(moqEventStore.Object);
            var eventBus = EventBus.Instance;
            eventBus.Execute(handledEvent);
        }
    }
}
