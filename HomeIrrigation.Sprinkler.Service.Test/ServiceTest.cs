using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HomeIrrigation.Weather.Service;
using NUnit.Framework;
using Should;
using Moq;
using System.Net.Http;
using Moq.Protected;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.Logging;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.Sprinkler.Service.DataTransferObjects.Commands.Irrigation;
using HomeIrrigation.ESFramework.Common.Base;
using System.Collections.Generic;
using HomeIrrigation.Common.Interfaces;
using HomeIrrigation.Sprinkler.Service.Handlers;
using HomeIrrigation.Common.CommandBus;
using HomeIrrigation.ESEvents.Common.Events;

namespace HomeIrrigation.Sprinkler.Service.Test
{
    [Parallelizable(ParallelScope.Children)]
    public class ServiceTest
    {
        private static IConfigurationRoot Configuration;
        Mock<IEventMetadata> moqEventMetadata;
        Mock<IEventStore> moqEventStore;

        [SetUp]
        public void Setup()
        {
            string path = GetPath();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(path))
                .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();

            moqEventMetadata = new Mock<IEventMetadata>();
            moqEventMetadata.Setup(x => x.Category).Returns("IRRIGATION");
            moqEventMetadata.SetupProperty(x => x.PublishedDateTime);
            moqEventMetadata.Setup(tenantId => tenantId.TenantId).Returns(Guid.NewGuid());

            moqEventStore = new Mock<IEventStore>();
            moqEventStore.Setup(x => x.SaveEvents(It.IsAny<CompositeAggregateId>(), It.IsAny<IEnumerable<IEvent>>()));
        }

        [Test]
        public void Given_No_Rain_Recorded_Should_Water_Lawn_For_60_Minutes()
        {
            // In hasn't rain in the past 7 days
            // The lawn has not been watered in the pass 7 days
            //Arrange
            string numberJson = ReadTestDataFile(Path.Combine("Data", "weatherRainFallData.txt"));
            
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                            {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(numberJson, Encoding.UTF8, "application/json"),
                            }));

            HttpClient httpClient = new HttpClient(httpMessageHandler.Object);
            httpClient.BaseAddress = new Uri(@"https://api.darksky.net/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var subjectUnderTest = new WeatherService(httpClient);
            DateTimeOffset now = DateTimeOffset.Now;
            var result = subjectUnderTest.GetRainfallInPastWeek(37.8267, -122.4233, now);
            result.ShouldNotBeSameAs(0);
            result.ShouldBeGreaterThan(0.0007d);
            var darkSkyKey = Configuration.GetSection("darkskykey").Value;
            //var expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-7).ToUnixTimeSeconds());
            var expectedUri = new Uri("https://api.open-meteo.com/v1/forecast?latitude=37.8267&longitude=-122.4233&hourly=temperature_2m,relative_humidity_2m,rain&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch&start_date=" + now.AddDays(-7).ToString("yyyy-MM-dd") + "&end_date=" + now.ToString("yyyy-MM-dd"));

            httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri),
                    ItExpr.IsAny<CancellationToken>());

            IrrigationCalculator ir = new IrrigationCalculator();
            var minutesToIrrigate = ir.HowLongToIrrigate(result, 0);

            // The lawn should be watered for 60 minutes
            minutesToIrrigate.ShouldEqual(1.5);
        }

        [Test]
        public void Given_Irrigation_Over_Irrigation_Limit_Should_Water_Lawn_For_0_Minutes()
        {
            // In hasn't rain in the past 7 days
            // The lawn has not been watered in the pass 7 days
            //Arrange
            string numberJson = ReadTestDataFile(Path.Combine("Data", "weatherRainFallData.txt"));

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                            {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(numberJson, Encoding.UTF8, "application/json"),
                            }));

            HttpClient httpClient = new HttpClient(httpMessageHandler.Object);
            httpClient.BaseAddress = new Uri(@"https://api.darksky.net/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var subjectUnderTest = new WeatherService(httpClient);
            DateTimeOffset now = DateTimeOffset.Now;
            var result = subjectUnderTest.GetRainfallInPastWeek(37.8267, -122.4233, now);
            result.ShouldNotBeSameAs(0);
            result.ShouldBeGreaterThan(0.0007d);
            var darkSkyKey = Configuration.GetSection("darkskykey").Value;
            //var expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-7).ToUnixTimeSeconds());
            var expectedUri = new Uri("https://api.open-meteo.com/v1/forecast?latitude=37.8267&longitude=-122.4233&hourly=temperature_2m,relative_humidity_2m,rain&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch&start_date=" + now.AddDays(-7).ToString("yyyy-MM-dd") + "&end_date=" + now.ToString("yyyy-MM-dd"));

            httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri),
                    ItExpr.IsAny<CancellationToken>());

            IrrigationCalculator ir = new IrrigationCalculator();
            var minutesToIrrigate = ir.HowLongToIrrigate(result, 2);

            // The lawn should not be watered
            minutesToIrrigate.ShouldEqual(0);
        }

        [Test]
        public void Given_That_It_Is_5AM_Determine_If_We_Need_To_Water_The_Lawn__We_Should_Water_The_Lawn_For_60_Minutes()
        {
            // In hasn't rain enough in the past 7 days
            // The lawn has not been watered enough in the pass 7 days
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var scheduler = new Mock<IScheduler>();
            string numberJson = ReadTestDataFile(Path.Combine("Data", "weatherRainFallData.txt"));
          

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            var eventStoreMock = new Mock<IEventStore>();
            moqEventMetadata.Setup(Id => Id.TenantId).Returns(Guid.NewGuid());

            var zones = new List<IEvent>();
            var zoneId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            StartIrrigationCommand cmd = new StartIrrigationCommand()
            {
                Zone = zoneId,
                TenantId = tenantId
            };
            EventMetadata md = new EventMetadata(tenantId, "IRRIGATION", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            zones.Add(new IrrigateZoneStarted(zoneId, DateTimeOffset.UtcNow, md, cmd.HowLongToIrrigate));

            moqEventMetadata.Setup(Id => Id.TenantId).Returns(tenantId);
            eventStoreMock.Setup(found => found.GetAllEvents(It.IsAny<CompositeAggregateId>())).Returns(zones);

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                            {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(numberJson, Encoding.UTF8, "application/json"),
                            }));

            HttpClient httpClient = new HttpClient(httpMessageHandler.Object);
            httpClient.BaseAddress = new Uri(@"https://api.darksky.net/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var timer = new Mock<ITimer>();
            var subjectUnderTest = new SprinklerService(mockLogger.Object, timer.Object, httpClient, eventStoreMock.Object, moqEventMetadata.Object.TenantId);
            timer.Raise(s => s.Elapsed += null, new object());
            DateTimeOffset now = DateTimeOffset.Now;
            var darkSkyKey = Configuration.GetSection("darkskykey").Value;
            //var expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.023434,-84.615494" + "," + now.AddDays(-1).ToUnixTimeSeconds());
            httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        /*&& req.RequestUri == expectedUri*/),
                    ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void Start_Irrigation_Should_Send_One_Event_To_EventStore()
        {
            CommandHandlerRegistration.RegisterCommandHandler();

            var tenantId = Guid.NewGuid();
            var zoneId = Guid.NewGuid();
            StartIrrigationCommand cmd = new StartIrrigationCommand()
            {
                Zone = zoneId,
                TenantId = tenantId
            };

            var zones = new List<IEvent>();
            moqEventMetadata.Setup(Id => Id.TenantId).Returns(tenantId);
            EventMetadata md = new EventMetadata(tenantId, "Zone", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            zones.Add(new IrrigateZoneStarted(zoneId, DateTimeOffset.UtcNow, md, cmd.HowLongToIrrigate));
            moqEventStore.Setup(found => found.GetAllEvents(It.IsAny<CompositeAggregateId>())).Returns(zones);

            var zone = new Domain.Zone(cmd.Zone, moqEventStore.Object);
            zone.StartIrrigation(cmd, moqEventMetadata.Object);

            moqEventStore.Verify(m => m.SaveEvents(It.IsAny<CompositeAggregateId>(), It.IsAny<IEnumerable<IEvent>>()), Times.Exactly(2));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [NonParallelizable]
        public void Start_Irrigation_Should_Run_For_X_Minutes(int irrigationTime)
        {
            CommandHandlerRegistration.RegisterCommandHandler();

            var tenantId = Guid.NewGuid();
            var zoneId = Guid.NewGuid();
            StartIrrigationCommand cmd = new StartIrrigationCommand()
            {
                Zone = zoneId,
                TenantId = tenantId,
                HowLongToIrrigate = irrigationTime
            };
            EventMetadata md = new EventMetadata(tenantId, "Zone", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            var zones = new List<IEvent>();
            zones.Add(new IrrigateZoneStarted(zoneId, DateTimeOffset.UtcNow, md, cmd.HowLongToIrrigate));

            moqEventMetadata.Setup(Id => Id.TenantId).Returns(tenantId);
            moqEventStore.Setup(found => found.GetAllEvents(It.IsAny<CompositeAggregateId>())).Returns(zones);

            DateTimeOffset startTimer = DateTimeOffset.UtcNow;

            var zone = new Domain.Zone(cmd.Zone, moqEventStore.Object);
            zone.StartIrrigation(cmd, moqEventMetadata.Object);

            DateTimeOffset endTime = DateTimeOffset.UtcNow;
            TimeSpan timeSpan = endTime - startTimer;

            timeSpan.Minutes.ShouldBeGreaterThanOrEqualTo((int)cmd.HowLongToIrrigate);
        }

        [Test]
        [NonParallelizable]
        public void Start_Irrigation_Should_Send_One_Event_For_Each_Zone_To_EventStore()
        {
            string numberJson = ReadTestDataFile(Path.Combine("Data", "weatherRainFallData.txt"));
           
            var zoneId = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var mockLogger = new Mock<ILogger>();
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            var eventStoreMock = new Mock<IEventStore>();
            EventMetadata md = new EventMetadata(tenantId, "IRRIGATION", Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            StartIrrigationCommand cmd = new StartIrrigationCommand()
            {
                Zone = zoneId,
                TenantId = tenantId
            };
            var zones = new List<IEvent>();
            zones.Add(new IrrigateZoneStarted(zoneId, DateTimeOffset.UtcNow, md, cmd.HowLongToIrrigate));
            eventStoreMock.Setup(found => found.GetAllEvents(It.IsAny<CompositeAggregateId>())).Returns(zones);

            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                            {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(numberJson, Encoding.UTF8, "application/json"),
                            })
                );

            HttpClient httpClient = new HttpClient(httpMessageHandler.Object);
            httpClient.BaseAddress = new Uri(@"https://api.darksky.net/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var timer = new Mock<ITimer>();
            var subjectUnderTest = new SprinklerService(mockLogger.Object, timer.Object, httpClient, eventStoreMock.Object, md.TenantId);
            timer.Raise(s => s.Elapsed += null, new object());
            eventStoreMock.Verify(m => m.SaveEvents(It.IsAny<CompositeAggregateId>(), It.IsAny<IEnumerable<IEvent>>()), Times.Exactly(6));
        }

        private static string GetPath()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return path;
        }
        private static async Task<string> ReadTestDataFileAsync(string relativePath)
        {
            var basePath = Path.GetDirectoryName(GetPath());
            var filePath = Path.Combine(basePath, relativePath);
            return await File.ReadAllTextAsync(filePath);
        }

        private static string ReadTestDataFile(string relativePath)
        {
            // base directory for the test assembly (GetPath already exists in this class)
            var basePath = Path.GetDirectoryName(GetPath());
            var filePath = Path.Combine(basePath, relativePath);
            return File.ReadAllText(filePath);
        }
    }
}
