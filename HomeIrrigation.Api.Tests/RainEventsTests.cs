using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.Api.DataTransferObjects.Commands.Rain;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.ESEvents.Common.Events;

namespace HomeIrrigation.Api.Test
{
    [TestFixture]
    public class RainEventsTests
    {
        EventMetadata eventMetadata;
        Mock<IEventStore> moqEventStore;

        [SetUp]
        public void Setup()
        {
            eventMetadata = new EventMetadata(Guid.NewGuid(), "TestCategory", "TestCorrelationId", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
            moqEventStore = new Mock<IEventStore>();
            moqEventStore.Setup(x => x.SaveEvents(It.IsAny<CompositeAggregateId>(), It.IsAny<IEnumerable<IEvent>>()));
        }

        [Test]
        public void Should_Generate_RainFell_Event_When_Recording_RainFall()
        {
            RainFallCommand cmd = new RainFallCommand(6.2);

            var rain = Domain.Rain.RecordRainfall(eventMetadata, moqEventStore.Object, cmd);

            var events = rain.GetUncommittedEvents();

            Assert.IsNotEmpty(events);
            Assert.AreEqual(1, events.Count());
            Assert.IsInstanceOf<RainFell>(events.First());
            var fell = events.First() as RainFell;
            Assert.AreEqual(6.2, fell.Inches);

        }
    }
}
