using NUnit.Framework;
using HomeIrrigation.Api.Domain;
using Moq;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.Api.DataTransferObjects.Irrigation;

namespace HomeIrrigation.Api.Test
{
    public class IrrigationEventTest
    {
        [Test]
        public void Should_Generate_IrrigateZone_Event_When_Zone_Needs_Watering()
        {
            var eventMetadataMock = new Mock<IEventMetadata>();
            var eventStoreMock = new Mock<IEventStore>();
            IrrigateZoneCommand cmd = new IrrigateZoneCommand()
            {
            };

            //var irrigation = Irrigation.IrrigateZone(eventMetadataMock.Object, eventStoreMock.Object, cmd);
            //var events = p.GetUncommittedEvents();
            //Assert.IsNotEmpty(events);            
        }
    }
}
