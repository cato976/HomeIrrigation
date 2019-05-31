using NUnit.Framework;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using System;
using Moq;
using HomeIrrigation.Sprinkler.Service;

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

            //var deviceRepoMock = new Mock<IDeviceRepository>();
            //var unitOfWorkFactoryMock = new Mock<IUnitOfWorkFactory>();
            //var unitOfWorkMock = new Mock<IUnitOfWork>();
            //var eventMetadataMock = new Mock<IEventMetadata>();
            //var mediStatContext = new MediStatContext();
            //unitOfWorkMock.Setup(x => x.DeviceRepository.Add(It.IsAny<DeviceMap>()));
            //unitOfWorkFactoryMock.Setup(x => x.Create()).Returns(unitOfWorkMock.Object);

            var eventMetadata = new EventMetadata();

            // Act

            //var unitOfWorkFactory = new UnitOfWorkFactory();
            var sprinklerService = new SprinklerService();
            var rainFall = new Data.DataAccess.Models.RainFall(Guid.NewGuid(), eventMetadata, 4);
            sprinklerService.RecordRain(rainFall);

            // Assert
            //unitOfWorkMock.Verify(m => m.DeviceRepository.InsertDevice(It.IsAny<MediStat.Data.DataAccess.Models.Device>()), Times.Once);
            //unitOfWorkMock.Verify(m => m.CommitAsync(), Times.Once);
            //unitOfWorkFactoryMock.Verify(m => m.Create(), Times.Once);
        }
    }
}
