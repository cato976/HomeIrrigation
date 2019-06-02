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
            eventMetadata = new EventMetadata(Guid.NewGuid(), "Rain", "TestCorrelationId", Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow);
            moqEventStore = new Mock<IEventStore>();
            moqEventStore.Setup(x => x.SaveEvents(It.IsAny<CompositeAggregateId>(), It.IsAny<IEnumerable<IEvent>>()));
        }

        [TestCase(6.2)]
        [TestCase(4.2)]
        [TestCase(1.2)]
        public void Should_Generate_RainFell_Event_When_Recording_RainFall(double inches)
        {
            var eventsList = new List<IEvent>();
            eventsList.Add(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(-9), eventMetadata, 5));
            eventsList.Add(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow, eventMetadata, inches));
            moqEventStore.Setup(x => x.GetAllEvents(It.IsAny<CompositeAggregateId>())).Returns((List<IEvent>)eventsList);

            RainFallCommand cmd = new RainFallCommand(inches);

            var rain = Domain.Rain.RecordRainfall(eventMetadata, moqEventStore.Object, cmd);
            var events = rain.GetUncommittedEvents();

            Assert.IsNotEmpty(events);
            Assert.AreEqual(1, events.Count());
            Assert.IsInstanceOf<RainFell>(events.First());
        }

        [TestCase(6.2, 7.4)]
        [TestCase(4.2, 1.1)]
        [TestCase(1.2, 4.3)]
        public void Should_Get_Weekly_RainFall(double inches, double secondRain)
        {
            var events = new List<IEvent>();
            events.Add(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow.AddDays(-9), eventMetadata, 5)); // Add some rain for the pass
            events.Add(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow, eventMetadata, inches));
            events.Add(new RainFell(Guid.NewGuid(), DateTimeOffset.UtcNow, eventMetadata, secondRain));
            moqEventStore.Setup(x => x.GetAllEvents(It.IsAny<CompositeAggregateId>())).Returns((List<IEvent>)events);

            Domain.Rain rain = null;
            foreach (var item in events)
            {
                RainFallCommand cmd = new RainFallCommand(((RainFell)item).Inches);

                rain = Domain.Rain.RecordRainfall(eventMetadata, moqEventStore.Object, cmd);
            }

            var weeklyRainfall = Domain.Rain.GetWeeklyRainfall(moqEventStore.Object,
                new CompositeAggregateId(eventMetadata.TenantId, rain.AggregateGuid, eventMetadata.Category));
            Assert.AreEqual(inches + secondRain, weeklyRainfall);
        }
    }
}
