using NUnit.Framework;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using System;
using Moq;
using HomeIrrigation.Sprinkler.Service;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace HomeIrrigation.Api.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Should_Record_Rain()
        {
            // Arrange

            var eventStoreMock = new Mock<IEventStore>();

            var eventMetadata = new EventMetadata();

            // Act

            var logger = new Mock<ILogger<HomeIrrigation.Sprinkler.Service.SprinklerService>>();
            HttpClient client = new HttpClient();
            var timer = new Mock<ITimer>();
            var sprinklerService = new SprinklerService(logger.Object, timer.Object, client, eventStoreMock.Object, eventMetadata.TenantId);
            var rainFall = new Data.DataAccess.Models.RainFall(Guid.NewGuid(), eventMetadata, 4);
            sprinklerService.RecordRain(rainFall);

            // Assert
            //unitOfWorkMock.Verify(m => m.DeviceRepository.InsertDevice(It.IsAny<MediStat.Data.DataAccess.Models.Device>()), Times.Once);
            //unitOfWorkMock.Verify(m => m.CommitAsync(), Times.Once);
            //unitOfWorkFactoryMock.Verify(m => m.Create(), Times.Once);
        }
    }
}
