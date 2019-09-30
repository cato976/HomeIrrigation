using NUnit.Framework;
using HomeIrrigation.Api.Domain;
using Moq;
using HomeIrrigation.ESFramework.Common.Interfaces;
using System.Linq;
using HomeIrrigation.Api.DataTransferObjects.Commands;
using System;
using HomeIrrigation.ESFramework.Common.Base;
using System.Collections.Generic;

namespace HomeIrrigation.Api.Test
{
    public class IrrigationEventTest
    {
        [Test]
        public void Should_Generate_IrrigateZone_Event_When_Zone_Needs_Watering()
        {
            Mock<IEventMetadata> eventMetadataMock = new Mock<IEventMetadata>();
            var eventStoreMock = new Mock<IEventStore>();

            eventMetadataMock.Setup(x => x.Category).Returns("Zone");
            eventMetadataMock.SetupProperty(x => x.PublishedDateTime);
            eventMetadataMock.Setup(x => x.TenantId).Returns(Guid.NewGuid());
            eventMetadataMock.Setup(x => x.CorrelationId).Returns(Guid.NewGuid());
            eventMetadataMock.Setup(x => x.AccountGuid).Returns(Guid.NewGuid());
            EventMetadata eventMetadata = new EventMetadata();

            IrrigateZoneCommand cmd = new IrrigateZoneCommand()
            {
            };

            var zone = new Zone();
            zone.IrrigateZone(eventMetadata, eventStoreMock.Object, cmd);
            var events = zone.GetUncommittedEvents();
            Assert.IsNotEmpty(events);
            Assert.AreEqual(1, events.Count());
            eventStoreMock.Verify(m => m.SaveEvents(It.IsAny<CompositeAggregateId>(), It.IsAny<IEnumerable<IEvent>>()), Times.Once);
        }
    }
}
