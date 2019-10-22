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

namespace HomeIrrigation.Sprinkler.Service.Test
{
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
            moqEventMetadata.Setup(x => x.Category).Returns("Zone");
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
            string numberJson = @"
            {
                ""latitude"": 37.8267,
                    ""longitude"": -122.4233,
                    ""timezone"": ""America/Los_Angeles"",
                    ""currently"": {
                        ""time"": 1567803629,
                        ""summary"": ""Clear"",
                        ""icon"": ""clear-day"",
                        ""precipIntensity"": 0,
                        ""precipProbability"": 0,
                        ""temperature"": 65.04,
                        ""apparentTemperature"": 65.04,
                        ""dewPoint"": 56.07,
                        ""humidity"": 0.73,
                        ""pressure"": 1018.82,
                        ""windSpeed"": 7.82,
                        ""windGust"": 12.54,
                        ""windBearing"": 248,
                        ""cloudCover"": 0,
                        ""uvIndex"": 8,
                        ""visibility"": 8.226,
                        ""ozone"": 283.9
                    },
                    ""hourly"": {
                        ""summary"": ""Mostly cloudy throughout the day."",
                        ""icon"": ""partly-cloudy-day"",
                        ""data"": [
                        {
                            ""time"": 1567753200,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.96,
                            ""apparentTemperature"": 60.96,
                            ""dewPoint"": 56.88,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.07,
                            ""windSpeed"": 7.36,
                            ""windGust"": 11.72,
                            ""windBearing"": 257,
                            ""cloudCover"": 0.62,
                            ""uvIndex"": 0,
                            ""visibility"": 9.602,
                            ""ozone"": 282.6
                        },
                        {
                            ""time"": 1567756800,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.69,
                            ""apparentTemperature"": 60.69,
                            ""dewPoint"": 56.43,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.15,
                            ""windSpeed"": 5.91,
                            ""windGust"": 9.7,
                            ""windBearing"": 233,
                            ""cloudCover"": 0.66,
                            ""uvIndex"": 0,
                            ""visibility"": 9.619,
                            ""ozone"": 281.1
                        },
                        {
                            ""time"": 1567760400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.33,
                            ""apparentTemperature"": 60.33,
                            ""dewPoint"": 56.24,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.05,
                            ""windSpeed"": 6,
                            ""windGust"": 9.28,
                            ""windBearing"": 237,
                            ""cloudCover"": 0.83,
                            ""uvIndex"": 0,
                            ""visibility"": 9.59,
                            ""ozone"": 280
                        },
                        {
                            ""time"": 1567764000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.22,
                            ""apparentTemperature"": 60.22,
                            ""dewPoint"": 56.36,
                            ""humidity"": 0.87,
                            ""pressure"": 1016.95,
                            ""windSpeed"": 5.95,
                            ""windGust"": 8.59,
                            ""windBearing"": 238,
                            ""cloudCover"": 0.87,
                            ""uvIndex"": 0,
                            ""visibility"": 9.995,
                            ""ozone"": 279.5
                        },
                        {
                            ""time"": 1567767600,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.73,
                            ""apparentTemperature"": 59.73,
                            ""dewPoint"": 56.35,
                            ""humidity"": 0.89,
                            ""pressure"": 1016.97,
                            ""windSpeed"": 6.32,
                            ""windGust"": 9.13,
                            ""windBearing"": 248,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 9.915,
                            ""ozone"": 279.3
                        },
                        {
                            ""time"": 1567771200,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.59,
                            ""apparentTemperature"": 59.59,
                            ""dewPoint"": 56.47,
                            ""humidity"": 0.89,
                            ""pressure"": 1017.07,
                            ""windSpeed"": 5.76,
                            ""windGust"": 8.29,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 9.705,
                            ""ozone"": 279.2
                        },
                        {
                            ""time"": 1567774800,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.04,
                            ""apparentTemperature"": 59.04,
                            ""dewPoint"": 56.45,
                            ""humidity"": 0.91,
                            ""pressure"": 1017.57,
                            ""windSpeed"": 5.39,
                            ""windGust"": 7.56,
                            ""windBearing"": 239,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 282.1
                        },
                        {
                            ""time"": 1567778400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.14,
                            ""apparentTemperature"": 59.14,
                            ""dewPoint"": 56.56,
                            ""humidity"": 0.91,
                            ""pressure"": 1017.75,
                            ""windSpeed"": 5.2,
                            ""windGust"": 7.62,
                            ""windBearing"": 214,
                            ""cloudCover"": 0.82,
                            ""uvIndex"": 0,
                            ""visibility"": 8.514,
                            ""ozone"": 282.9
                        },
                        {
                            ""time"": 1567782000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.55,
                            ""apparentTemperature"": 58.55,
                            ""dewPoint"": 56.54,
                            ""humidity"": 0.93,
                            ""pressure"": 1018.59,
                            ""windSpeed"": 4.95,
                            ""windGust"": 7.54,
                            ""windBearing"": 224,
                            ""cloudCover"": 0.86,
                            ""uvIndex"": 0,
                            ""visibility"": 6.634,
                            ""ozone"": 283.4
                        },
                        {
                            ""time"": 1567785600,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.4,
                            ""apparentTemperature"": 58.4,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.94,
                            ""pressure"": 1019.13,
                            ""windSpeed"": 4.71,
                            ""windGust"": 7.65,
                            ""windBearing"": 229,
                            ""cloudCover"": 0.94,
                            ""uvIndex"": 1,
                            ""visibility"": 9.296,
                            ""ozone"": 283.3
                        },
                        {
                            ""time"": 1567789200,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.86,
                            ""apparentTemperature"": 58.86,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.92,
                            ""pressure"": 1019.09,
                            ""windSpeed"": 5.8,
                            ""windGust"": 8.94,
                            ""windBearing"": 225,
                            ""cloudCover"": 0.79,
                            ""uvIndex"": 3,
                            ""visibility"": 7.308,
                            ""ozone"": 282.9
                        },
                        {
                            ""time"": 1567792800,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.71,
                            ""apparentTemperature"": 59.71,
                            ""dewPoint"": 56.5,
                            ""humidity"": 0.89,
                            ""pressure"": 1019.27,
                            ""windSpeed"": 6.15,
                            ""windGust"": 9.31,
                            ""windBearing"": 231,
                            ""cloudCover"": 0.7,
                            ""uvIndex"": 4,
                            ""visibility"": 9.725,
                            ""ozone"": 284.3
                        },
                        {
                            ""time"": 1567796400,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 61.85,
                            ""apparentTemperature"": 61.85,
                            ""dewPoint"": 56.4,
                            ""humidity"": 0.82,
                            ""pressure"": 1019.69,
                            ""windSpeed"": 6.91,
                            ""windGust"": 10.58,
                            ""windBearing"": 236,
                            ""cloudCover"": 0.19,
                            ""uvIndex"": 7,
                            ""visibility"": 7.918,
                            ""ozone"": 284.3
                        },
                        {
                            ""time"": 1567800000,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 63.09,
                            ""apparentTemperature"": 63.09,
                            ""dewPoint"": 56.18,
                            ""humidity"": 0.78,
                            ""pressure"": 1018.97,
                            ""windSpeed"": 7.51,
                            ""windGust"": 11.69,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.13,
                            ""uvIndex"": 8,
                            ""visibility"": 7.781,
                            ""ozone"": 284.2
                        },
                        {
                            ""time"": 1567803600,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.04,
                            ""apparentTemperature"": 65.04,
                            ""dewPoint"": 56.07,
                            ""humidity"": 0.73,
                            ""pressure"": 1018.83,
                            ""windSpeed"": 7.82,
                            ""windGust"": 12.55,
                            ""windBearing"": 248,
                            ""cloudCover"": 0,
                            ""uvIndex"": 8,
                            ""visibility"": 8.228,
                            ""ozone"": 283.9
                        },
                        {
                            ""time"": 1567807200,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.65,
                            ""apparentTemperature"": 65.65,
                            ""dewPoint"": 56.16,
                            ""humidity"": 0.71,
                            ""pressure"": 1018.28,
                            ""windSpeed"": 7.42,
                            ""windGust"": 11.72,
                            ""windBearing"": 224,
                            ""cloudCover"": 0,
                            ""uvIndex"": 7,
                            ""visibility"": 8.051,
                            ""ozone"": 283.4
                        },
                        {
                            ""time"": 1567810800,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 66.61,
                            ""apparentTemperature"": 66.61,
                            ""dewPoint"": 56.26,
                            ""humidity"": 0.69,
                            ""pressure"": 1018.26,
                            ""windSpeed"": 7.69,
                            ""windGust"": 11.01,
                            ""windBearing"": 278,
                            ""cloudCover"": 0,
                            ""uvIndex"": 4,
                            ""visibility"": 8.98,
                            ""ozone"": 282.8
                        },
                        {
                            ""time"": 1567814400,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 67.27,
                            ""apparentTemperature"": 67.27,
                            ""dewPoint"": 55.96,
                            ""humidity"": 0.67,
                            ""pressure"": 1017.92,
                            ""windSpeed"": 7.5,
                            ""windGust"": 11.1,
                            ""windBearing"": 261,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 2,
                            ""visibility"": 9.591,
                            ""ozone"": 287.3
                        },
                        {
                            ""time"": 1567818000,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.5,
                            ""apparentTemperature"": 65.5,
                            ""dewPoint"": 56.44,
                            ""humidity"": 0.73,
                            ""pressure"": 1017.56,
                            ""windSpeed"": 8.3,
                            ""windGust"": 11.45,
                            ""windBearing"": 260,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 1,
                            ""visibility"": 9.483,
                            ""ozone"": 285.9
                        },
                        {
                            ""time"": 1567821600,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 64.22,
                            ""apparentTemperature"": 64.22,
                            ""dewPoint"": 55.7,
                            ""humidity"": 0.74,
                            ""pressure"": 1017.31,
                            ""windSpeed"": 8.11,
                            ""windGust"": 10.97,
                            ""windBearing"": 254,
                            ""cloudCover"": 0.09,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 283.6
                        },
                        {
                            ""time"": 1567825200,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 62.88,
                            ""apparentTemperature"": 62.88,
                            ""dewPoint"": 55.68,
                            ""humidity"": 0.77,
                            ""pressure"": 1017.36,
                            ""windSpeed"": 7.52,
                            ""windGust"": 10.62,
                            ""windBearing"": 257,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 0,
                            ""visibility"": 9.173,
                            ""ozone"": 281.6
                        },
                        {
                            ""time"": 1567828800,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 62.22,
                            ""apparentTemperature"": 62.22,
                            ""dewPoint"": 56.12,
                            ""humidity"": 0.8,
                            ""pressure"": 1017.52,
                            ""windSpeed"": 6.16,
                            ""windGust"": 8.49,
                            ""windBearing"": 262,
                            ""cloudCover"": 0.13,
                            ""uvIndex"": 0,
                            ""visibility"": 9.43,
                            ""ozone"": 280.7
                        },
                        {
                            ""time"": 1567832400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 61.03,
                            ""apparentTemperature"": 61.03,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.85,
                            ""pressure"": 1017.9,
                            ""windSpeed"": 6.59,
                            ""windGust"": 9.34,
                            ""windBearing"": 233,
                            ""cloudCover"": 0.54,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 280.1
                        },
                        {
                            ""time"": 1567836000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.65,
                            ""apparentTemperature"": 60.65,
                            ""dewPoint"": 56,
                            ""humidity"": 0.85,
                            ""pressure"": 1017.83,
                            ""windSpeed"": 6.6,
                            ""windGust"": 9.3,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.67,
                            ""uvIndex"": 0,
                            ""visibility"": 8.91,
                            ""ozone"": 280.8
                        }
                        ]
                    },
                    ""daily"": {
                        ""data"": [
                        {
                            ""time"": 1567753200,
                            ""summary"": ""Mostly cloudy throughout the day."",
                            ""icon"": ""partly-cloudy-day"",
                            ""sunriseTime"": 1567777528,
                            ""sunsetTime"": 1567823613,
                            ""moonPhase"": 0.28,
                            ""precipIntensity"": 0,
                            ""precipIntensityMax"": 0.0001,
                            ""precipIntensityMaxTime"": 1567832400,
                            ""precipProbability"": 0.04,
                            ""temperatureHigh"": 67.27,
                            ""temperatureHighTime"": 1567814400,
                            ""temperatureLow"": 58.89,
                            ""temperatureLowTime"": 1567868400,
                            ""apparentTemperatureHigh"": 67.27,
                            ""apparentTemperatureHighTime"": 1567814400,
                            ""apparentTemperatureLow"": 58.89,
                            ""apparentTemperatureLowTime"": 1567868400,
                            ""dewPoint"": 56.31,
                            ""humidity"": 0.83,
                            ""pressure"": 1017.96,
                            ""windSpeed"": 6.57,
                            ""windGust"": 12.55,
                            ""windGustTime"": 1567803600,
                            ""windBearing"": 244,
                            ""cloudCover"": 0.49,
                            ""uvIndex"": 8,
                            ""uvIndexTime"": 1567803600,
                            ""visibility"": 9.06,
                            ""ozone"": 282.5,
                            ""temperatureMin"": 58.4,
                            ""temperatureMinTime"": 1567785600,
                            ""temperatureMax"": 67.27,
                            ""temperatureMaxTime"": 1567814400,
                            ""apparentTemperatureMin"": 58.4,
                            ""apparentTemperatureMinTime"": 1567785600,
                            ""apparentTemperatureMax"": 67.27,
                            ""apparentTemperatureMaxTime"": 1567814400
                        }
                        ]
                    },
                    ""flags"": {
                        ""sources"": [
                            ""cmc"",
                        ""gfs"",
                        ""hrrr"",
                        ""icon"",
                        ""isd"",
                        ""madis"",
                        ""nam"",
                        ""sref""
                        ],
                        ""nearest-station"": 1.835,
                        ""units"": ""us""
                    },
                    ""offset"": -7
            }";

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
            var expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-7).ToUnixTimeSeconds());

            httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(7),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri),
                    ItExpr.IsAny<CancellationToken>());

            IrrigationCalculator ir = new IrrigationCalculator();
            var minutesToIrrigate = ir.HowLongToIrrigate(result, 0);

            // The lawn should be watered for 60 minutes
            minutesToIrrigate.ShouldEqual(60);
        }

        [Test]
        public void Given_Irrigation_Over_Irrigation_Limit_Should_Water_Lawn_For_0_Minutes()
        {
            // In hasn't rain in the past 7 days
            // The lawn has not been watered in the pass 7 days
            //Arrange
            string numberJson = @"
            {
                ""latitude"": 37.8267,
                    ""longitude"": -122.4233,
                    ""timezone"": ""America/Los_Angeles"",
                    ""currently"": {
                        ""time"": 1567803629,
                        ""summary"": ""Clear"",
                        ""icon"": ""clear-day"",
                        ""precipIntensity"": 0,
                        ""precipProbability"": 0,
                        ""temperature"": 65.04,
                        ""apparentTemperature"": 65.04,
                        ""dewPoint"": 56.07,
                        ""humidity"": 0.73,
                        ""pressure"": 1018.82,
                        ""windSpeed"": 7.82,
                        ""windGust"": 12.54,
                        ""windBearing"": 248,
                        ""cloudCover"": 0,
                        ""uvIndex"": 8,
                        ""visibility"": 8.226,
                        ""ozone"": 283.9
                    },
                    ""hourly"": {
                        ""summary"": ""Mostly cloudy throughout the day."",
                        ""icon"": ""partly-cloudy-day"",
                        ""data"": [
                        {
                            ""time"": 1567753200,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.96,
                            ""apparentTemperature"": 60.96,
                            ""dewPoint"": 56.88,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.07,
                            ""windSpeed"": 7.36,
                            ""windGust"": 11.72,
                            ""windBearing"": 257,
                            ""cloudCover"": 0.62,
                            ""uvIndex"": 0,
                            ""visibility"": 9.602,
                            ""ozone"": 282.6
                        },
                        {
                            ""time"": 1567756800,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.69,
                            ""apparentTemperature"": 60.69,
                            ""dewPoint"": 56.43,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.15,
                            ""windSpeed"": 5.91,
                            ""windGust"": 9.7,
                            ""windBearing"": 233,
                            ""cloudCover"": 0.66,
                            ""uvIndex"": 0,
                            ""visibility"": 9.619,
                            ""ozone"": 281.1
                        },
                        {
                            ""time"": 1567760400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.33,
                            ""apparentTemperature"": 60.33,
                            ""dewPoint"": 56.24,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.05,
                            ""windSpeed"": 6,
                            ""windGust"": 9.28,
                            ""windBearing"": 237,
                            ""cloudCover"": 0.83,
                            ""uvIndex"": 0,
                            ""visibility"": 9.59,
                            ""ozone"": 280
                        },
                        {
                            ""time"": 1567764000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.22,
                            ""apparentTemperature"": 60.22,
                            ""dewPoint"": 56.36,
                            ""humidity"": 0.87,
                            ""pressure"": 1016.95,
                            ""windSpeed"": 5.95,
                            ""windGust"": 8.59,
                            ""windBearing"": 238,
                            ""cloudCover"": 0.87,
                            ""uvIndex"": 0,
                            ""visibility"": 9.995,
                            ""ozone"": 279.5
                        },
                        {
                            ""time"": 1567767600,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.73,
                            ""apparentTemperature"": 59.73,
                            ""dewPoint"": 56.35,
                            ""humidity"": 0.89,
                            ""pressure"": 1016.97,
                            ""windSpeed"": 6.32,
                            ""windGust"": 9.13,
                            ""windBearing"": 248,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 9.915,
                            ""ozone"": 279.3
                        },
                        {
                            ""time"": 1567771200,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.59,
                            ""apparentTemperature"": 59.59,
                            ""dewPoint"": 56.47,
                            ""humidity"": 0.89,
                            ""pressure"": 1017.07,
                            ""windSpeed"": 5.76,
                            ""windGust"": 8.29,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 9.705,
                            ""ozone"": 279.2
                        },
                        {
                            ""time"": 1567774800,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.04,
                            ""apparentTemperature"": 59.04,
                            ""dewPoint"": 56.45,
                            ""humidity"": 0.91,
                            ""pressure"": 1017.57,
                            ""windSpeed"": 5.39,
                            ""windGust"": 7.56,
                            ""windBearing"": 239,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 282.1
                        },
                        {
                            ""time"": 1567778400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.14,
                            ""apparentTemperature"": 59.14,
                            ""dewPoint"": 56.56,
                            ""humidity"": 0.91,
                            ""pressure"": 1017.75,
                            ""windSpeed"": 5.2,
                            ""windGust"": 7.62,
                            ""windBearing"": 214,
                            ""cloudCover"": 0.82,
                            ""uvIndex"": 0,
                            ""visibility"": 8.514,
                            ""ozone"": 282.9
                        },
                        {
                            ""time"": 1567782000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.55,
                            ""apparentTemperature"": 58.55,
                            ""dewPoint"": 56.54,
                            ""humidity"": 0.93,
                            ""pressure"": 1018.59,
                            ""windSpeed"": 4.95,
                            ""windGust"": 7.54,
                            ""windBearing"": 224,
                            ""cloudCover"": 0.86,
                            ""uvIndex"": 0,
                            ""visibility"": 6.634,
                            ""ozone"": 283.4
                        },
                        {
                            ""time"": 1567785600,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.4,
                            ""apparentTemperature"": 58.4,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.94,
                            ""pressure"": 1019.13,
                            ""windSpeed"": 4.71,
                            ""windGust"": 7.65,
                            ""windBearing"": 229,
                            ""cloudCover"": 0.94,
                            ""uvIndex"": 1,
                            ""visibility"": 9.296,
                            ""ozone"": 283.3
                        },
                        {
                            ""time"": 1567789200,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.86,
                            ""apparentTemperature"": 58.86,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.92,
                            ""pressure"": 1019.09,
                            ""windSpeed"": 5.8,
                            ""windGust"": 8.94,
                            ""windBearing"": 225,
                            ""cloudCover"": 0.79,
                            ""uvIndex"": 3,
                            ""visibility"": 7.308,
                            ""ozone"": 282.9
                        },
                        {
                            ""time"": 1567792800,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.71,
                            ""apparentTemperature"": 59.71,
                            ""dewPoint"": 56.5,
                            ""humidity"": 0.89,
                            ""pressure"": 1019.27,
                            ""windSpeed"": 6.15,
                            ""windGust"": 9.31,
                            ""windBearing"": 231,
                            ""cloudCover"": 0.7,
                            ""uvIndex"": 4,
                            ""visibility"": 9.725,
                            ""ozone"": 284.3
                        },
                        {
                            ""time"": 1567796400,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 61.85,
                            ""apparentTemperature"": 61.85,
                            ""dewPoint"": 56.4,
                            ""humidity"": 0.82,
                            ""pressure"": 1019.69,
                            ""windSpeed"": 6.91,
                            ""windGust"": 10.58,
                            ""windBearing"": 236,
                            ""cloudCover"": 0.19,
                            ""uvIndex"": 7,
                            ""visibility"": 7.918,
                            ""ozone"": 284.3
                        },
                        {
                            ""time"": 1567800000,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 63.09,
                            ""apparentTemperature"": 63.09,
                            ""dewPoint"": 56.18,
                            ""humidity"": 0.78,
                            ""pressure"": 1018.97,
                            ""windSpeed"": 7.51,
                            ""windGust"": 11.69,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.13,
                            ""uvIndex"": 8,
                            ""visibility"": 7.781,
                            ""ozone"": 284.2
                        },
                        {
                            ""time"": 1567803600,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.04,
                            ""apparentTemperature"": 65.04,
                            ""dewPoint"": 56.07,
                            ""humidity"": 0.73,
                            ""pressure"": 1018.83,
                            ""windSpeed"": 7.82,
                            ""windGust"": 12.55,
                            ""windBearing"": 248,
                            ""cloudCover"": 0,
                            ""uvIndex"": 8,
                            ""visibility"": 8.228,
                            ""ozone"": 283.9
                        },
                        {
                            ""time"": 1567807200,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.65,
                            ""apparentTemperature"": 65.65,
                            ""dewPoint"": 56.16,
                            ""humidity"": 0.71,
                            ""pressure"": 1018.28,
                            ""windSpeed"": 7.42,
                            ""windGust"": 11.72,
                            ""windBearing"": 224,
                            ""cloudCover"": 0,
                            ""uvIndex"": 7,
                            ""visibility"": 8.051,
                            ""ozone"": 283.4
                        },
                        {
                            ""time"": 1567810800,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 66.61,
                            ""apparentTemperature"": 66.61,
                            ""dewPoint"": 56.26,
                            ""humidity"": 0.69,
                            ""pressure"": 1018.26,
                            ""windSpeed"": 7.69,
                            ""windGust"": 11.01,
                            ""windBearing"": 278,
                            ""cloudCover"": 0,
                            ""uvIndex"": 4,
                            ""visibility"": 8.98,
                            ""ozone"": 282.8
                        },
                        {
                            ""time"": 1567814400,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 67.27,
                            ""apparentTemperature"": 67.27,
                            ""dewPoint"": 55.96,
                            ""humidity"": 0.67,
                            ""pressure"": 1017.92,
                            ""windSpeed"": 7.5,
                            ""windGust"": 11.1,
                            ""windBearing"": 261,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 2,
                            ""visibility"": 9.591,
                            ""ozone"": 287.3
                        },
                        {
                            ""time"": 1567818000,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.5,
                            ""apparentTemperature"": 65.5,
                            ""dewPoint"": 56.44,
                            ""humidity"": 0.73,
                            ""pressure"": 1017.56,
                            ""windSpeed"": 8.3,
                            ""windGust"": 11.45,
                            ""windBearing"": 260,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 1,
                            ""visibility"": 9.483,
                            ""ozone"": 285.9
                        },
                        {
                            ""time"": 1567821600,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 64.22,
                            ""apparentTemperature"": 64.22,
                            ""dewPoint"": 55.7,
                            ""humidity"": 0.74,
                            ""pressure"": 1017.31,
                            ""windSpeed"": 8.11,
                            ""windGust"": 10.97,
                            ""windBearing"": 254,
                            ""cloudCover"": 0.09,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 283.6
                        },
                        {
                            ""time"": 1567825200,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 62.88,
                            ""apparentTemperature"": 62.88,
                            ""dewPoint"": 55.68,
                            ""humidity"": 0.77,
                            ""pressure"": 1017.36,
                            ""windSpeed"": 7.52,
                            ""windGust"": 10.62,
                            ""windBearing"": 257,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 0,
                            ""visibility"": 9.173,
                            ""ozone"": 281.6
                        },
                        {
                            ""time"": 1567828800,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 62.22,
                            ""apparentTemperature"": 62.22,
                            ""dewPoint"": 56.12,
                            ""humidity"": 0.8,
                            ""pressure"": 1017.52,
                            ""windSpeed"": 6.16,
                            ""windGust"": 8.49,
                            ""windBearing"": 262,
                            ""cloudCover"": 0.13,
                            ""uvIndex"": 0,
                            ""visibility"": 9.43,
                            ""ozone"": 280.7
                        },
                        {
                            ""time"": 1567832400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 61.03,
                            ""apparentTemperature"": 61.03,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.85,
                            ""pressure"": 1017.9,
                            ""windSpeed"": 6.59,
                            ""windGust"": 9.34,
                            ""windBearing"": 233,
                            ""cloudCover"": 0.54,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 280.1
                        },
                        {
                            ""time"": 1567836000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.65,
                            ""apparentTemperature"": 60.65,
                            ""dewPoint"": 56,
                            ""humidity"": 0.85,
                            ""pressure"": 1017.83,
                            ""windSpeed"": 6.6,
                            ""windGust"": 9.3,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.67,
                            ""uvIndex"": 0,
                            ""visibility"": 8.91,
                            ""ozone"": 280.8
                        }
                        ]
                    },
                    ""daily"": {
                        ""data"": [
                        {
                            ""time"": 1567753200,
                            ""summary"": ""Mostly cloudy throughout the day."",
                            ""icon"": ""partly-cloudy-day"",
                            ""sunriseTime"": 1567777528,
                            ""sunsetTime"": 1567823613,
                            ""moonPhase"": 0.28,
                            ""precipIntensity"": 0,
                            ""precipIntensityMax"": 0.0001,
                            ""precipIntensityMaxTime"": 1567832400,
                            ""precipProbability"": 0.04,
                            ""temperatureHigh"": 67.27,
                            ""temperatureHighTime"": 1567814400,
                            ""temperatureLow"": 58.89,
                            ""temperatureLowTime"": 1567868400,
                            ""apparentTemperatureHigh"": 67.27,
                            ""apparentTemperatureHighTime"": 1567814400,
                            ""apparentTemperatureLow"": 58.89,
                            ""apparentTemperatureLowTime"": 1567868400,
                            ""dewPoint"": 56.31,
                            ""humidity"": 0.83,
                            ""pressure"": 1017.96,
                            ""windSpeed"": 6.57,
                            ""windGust"": 12.55,
                            ""windGustTime"": 1567803600,
                            ""windBearing"": 244,
                            ""cloudCover"": 0.49,
                            ""uvIndex"": 8,
                            ""uvIndexTime"": 1567803600,
                            ""visibility"": 9.06,
                            ""ozone"": 282.5,
                            ""temperatureMin"": 58.4,
                            ""temperatureMinTime"": 1567785600,
                            ""temperatureMax"": 67.27,
                            ""temperatureMaxTime"": 1567814400,
                            ""apparentTemperatureMin"": 58.4,
                            ""apparentTemperatureMinTime"": 1567785600,
                            ""apparentTemperatureMax"": 67.27,
                            ""apparentTemperatureMaxTime"": 1567814400
                        }
                        ]
                    },
                    ""flags"": {
                        ""sources"": [
                            ""cmc"",
                        ""gfs"",
                        ""hrrr"",
                        ""icon"",
                        ""isd"",
                        ""madis"",
                        ""nam"",
                        ""sref""
                        ],
                        ""nearest-station"": 1.835,
                        ""units"": ""us""
                    },
                    ""offset"": -7
            }";

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
            var expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-7).ToUnixTimeSeconds());

            httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(7),
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
            // In hasn't rain in the past 7 days
            // The lawn has not been watered in the pass 7 days
            //Arrange
            var mockLogger = new Mock<ILogger>();
            var scheduler = new Mock<IScheduler>();
            string numberJson = @"
            {
                ""latitude"": 37.8267,
                    ""longitude"": -122.4233,
                    ""timezone"": ""America/Los_Angeles"",
                    ""currently"": {
                        ""time"": 1567803629,
                        ""summary"": ""Clear"",
                        ""icon"": ""clear-day"",
                        ""precipIntensity"": 0,
                        ""precipProbability"": 0,
                        ""temperature"": 65.04,
                        ""apparentTemperature"": 65.04,
                        ""dewPoint"": 56.07,
                        ""humidity"": 0.73,
                        ""pressure"": 1018.82,
                        ""windSpeed"": 7.82,
                        ""windGust"": 12.54,
                        ""windBearing"": 248,
                        ""cloudCover"": 0,
                        ""uvIndex"": 8,
                        ""visibility"": 8.226,
                        ""ozone"": 283.9
                    },
                    ""hourly"": {
                        ""summary"": ""Mostly cloudy throughout the day."",
                        ""icon"": ""partly-cloudy-day"",
                        ""data"": [
                        {
                            ""time"": 1567753200,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.96,
                            ""apparentTemperature"": 60.96,
                            ""dewPoint"": 56.88,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.07,
                            ""windSpeed"": 7.36,
                            ""windGust"": 11.72,
                            ""windBearing"": 257,
                            ""cloudCover"": 0.62,
                            ""uvIndex"": 0,
                            ""visibility"": 9.602,
                            ""ozone"": 282.6
                        },
                        {
                            ""time"": 1567756800,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.69,
                            ""apparentTemperature"": 60.69,
                            ""dewPoint"": 56.43,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.15,
                            ""windSpeed"": 5.91,
                            ""windGust"": 9.7,
                            ""windBearing"": 233,
                            ""cloudCover"": 0.66,
                            ""uvIndex"": 0,
                            ""visibility"": 9.619,
                            ""ozone"": 281.1
                        },
                        {
                            ""time"": 1567760400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.33,
                            ""apparentTemperature"": 60.33,
                            ""dewPoint"": 56.24,
                            ""humidity"": 0.86,
                            ""pressure"": 1017.05,
                            ""windSpeed"": 6,
                            ""windGust"": 9.28,
                            ""windBearing"": 237,
                            ""cloudCover"": 0.83,
                            ""uvIndex"": 0,
                            ""visibility"": 9.59,
                            ""ozone"": 280
                        },
                        {
                            ""time"": 1567764000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.22,
                            ""apparentTemperature"": 60.22,
                            ""dewPoint"": 56.36,
                            ""humidity"": 0.87,
                            ""pressure"": 1016.95,
                            ""windSpeed"": 5.95,
                            ""windGust"": 8.59,
                            ""windBearing"": 238,
                            ""cloudCover"": 0.87,
                            ""uvIndex"": 0,
                            ""visibility"": 9.995,
                            ""ozone"": 279.5
                        },
                        {
                            ""time"": 1567767600,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.73,
                            ""apparentTemperature"": 59.73,
                            ""dewPoint"": 56.35,
                            ""humidity"": 0.89,
                            ""pressure"": 1016.97,
                            ""windSpeed"": 6.32,
                            ""windGust"": 9.13,
                            ""windBearing"": 248,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 9.915,
                            ""ozone"": 279.3
                        },
                        {
                            ""time"": 1567771200,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.59,
                            ""apparentTemperature"": 59.59,
                            ""dewPoint"": 56.47,
                            ""humidity"": 0.89,
                            ""pressure"": 1017.07,
                            ""windSpeed"": 5.76,
                            ""windGust"": 8.29,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 9.705,
                            ""ozone"": 279.2
                        },
                        {
                            ""time"": 1567774800,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.04,
                            ""apparentTemperature"": 59.04,
                            ""dewPoint"": 56.45,
                            ""humidity"": 0.91,
                            ""pressure"": 1017.57,
                            ""windSpeed"": 5.39,
                            ""windGust"": 7.56,
                            ""windBearing"": 239,
                            ""cloudCover"": 0.91,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 282.1
                        },
                        {
                            ""time"": 1567778400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.14,
                            ""apparentTemperature"": 59.14,
                            ""dewPoint"": 56.56,
                            ""humidity"": 0.91,
                            ""pressure"": 1017.75,
                            ""windSpeed"": 5.2,
                            ""windGust"": 7.62,
                            ""windBearing"": 214,
                            ""cloudCover"": 0.82,
                            ""uvIndex"": 0,
                            ""visibility"": 8.514,
                            ""ozone"": 282.9
                        },
                        {
                            ""time"": 1567782000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.55,
                            ""apparentTemperature"": 58.55,
                            ""dewPoint"": 56.54,
                            ""humidity"": 0.93,
                            ""pressure"": 1018.59,
                            ""windSpeed"": 4.95,
                            ""windGust"": 7.54,
                            ""windBearing"": 224,
                            ""cloudCover"": 0.86,
                            ""uvIndex"": 0,
                            ""visibility"": 6.634,
                            ""ozone"": 283.4
                        },
                        {
                            ""time"": 1567785600,
                            ""summary"": ""Overcast"",
                            ""icon"": ""cloudy"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.4,
                            ""apparentTemperature"": 58.4,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.94,
                            ""pressure"": 1019.13,
                            ""windSpeed"": 4.71,
                            ""windGust"": 7.65,
                            ""windBearing"": 229,
                            ""cloudCover"": 0.94,
                            ""uvIndex"": 1,
                            ""visibility"": 9.296,
                            ""ozone"": 283.3
                        },
                        {
                            ""time"": 1567789200,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 58.86,
                            ""apparentTemperature"": 58.86,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.92,
                            ""pressure"": 1019.09,
                            ""windSpeed"": 5.8,
                            ""windGust"": 8.94,
                            ""windBearing"": 225,
                            ""cloudCover"": 0.79,
                            ""uvIndex"": 3,
                            ""visibility"": 7.308,
                            ""ozone"": 282.9
                        },
                        {
                            ""time"": 1567792800,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 59.71,
                            ""apparentTemperature"": 59.71,
                            ""dewPoint"": 56.5,
                            ""humidity"": 0.89,
                            ""pressure"": 1019.27,
                            ""windSpeed"": 6.15,
                            ""windGust"": 9.31,
                            ""windBearing"": 231,
                            ""cloudCover"": 0.7,
                            ""uvIndex"": 4,
                            ""visibility"": 9.725,
                            ""ozone"": 284.3
                        },
                        {
                            ""time"": 1567796400,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 61.85,
                            ""apparentTemperature"": 61.85,
                            ""dewPoint"": 56.4,
                            ""humidity"": 0.82,
                            ""pressure"": 1019.69,
                            ""windSpeed"": 6.91,
                            ""windGust"": 10.58,
                            ""windBearing"": 236,
                            ""cloudCover"": 0.19,
                            ""uvIndex"": 7,
                            ""visibility"": 7.918,
                            ""ozone"": 284.3
                        },
                        {
                            ""time"": 1567800000,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 63.09,
                            ""apparentTemperature"": 63.09,
                            ""dewPoint"": 56.18,
                            ""humidity"": 0.78,
                            ""pressure"": 1018.97,
                            ""windSpeed"": 7.51,
                            ""windGust"": 11.69,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.13,
                            ""uvIndex"": 8,
                            ""visibility"": 7.781,
                            ""ozone"": 284.2
                        },
                        {
                            ""time"": 1567803600,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.04,
                            ""apparentTemperature"": 65.04,
                            ""dewPoint"": 56.07,
                            ""humidity"": 0.73,
                            ""pressure"": 1018.83,
                            ""windSpeed"": 7.82,
                            ""windGust"": 12.55,
                            ""windBearing"": 248,
                            ""cloudCover"": 0,
                            ""uvIndex"": 8,
                            ""visibility"": 8.228,
                            ""ozone"": 283.9
                        },
                        {
                            ""time"": 1567807200,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.65,
                            ""apparentTemperature"": 65.65,
                            ""dewPoint"": 56.16,
                            ""humidity"": 0.71,
                            ""pressure"": 1018.28,
                            ""windSpeed"": 7.42,
                            ""windGust"": 11.72,
                            ""windBearing"": 224,
                            ""cloudCover"": 0,
                            ""uvIndex"": 7,
                            ""visibility"": 8.051,
                            ""ozone"": 283.4
                        },
                        {
                            ""time"": 1567810800,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 66.61,
                            ""apparentTemperature"": 66.61,
                            ""dewPoint"": 56.26,
                            ""humidity"": 0.69,
                            ""pressure"": 1018.26,
                            ""windSpeed"": 7.69,
                            ""windGust"": 11.01,
                            ""windBearing"": 278,
                            ""cloudCover"": 0,
                            ""uvIndex"": 4,
                            ""visibility"": 8.98,
                            ""ozone"": 282.8
                        },
                        {
                            ""time"": 1567814400,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 67.27,
                            ""apparentTemperature"": 67.27,
                            ""dewPoint"": 55.96,
                            ""humidity"": 0.67,
                            ""pressure"": 1017.92,
                            ""windSpeed"": 7.5,
                            ""windGust"": 11.1,
                            ""windBearing"": 261,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 2,
                            ""visibility"": 9.591,
                            ""ozone"": 287.3
                        },
                        {
                            ""time"": 1567818000,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 65.5,
                            ""apparentTemperature"": 65.5,
                            ""dewPoint"": 56.44,
                            ""humidity"": 0.73,
                            ""pressure"": 1017.56,
                            ""windSpeed"": 8.3,
                            ""windGust"": 11.45,
                            ""windBearing"": 260,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 1,
                            ""visibility"": 9.483,
                            ""ozone"": 285.9
                        },
                        {
                            ""time"": 1567821600,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 64.22,
                            ""apparentTemperature"": 64.22,
                            ""dewPoint"": 55.7,
                            ""humidity"": 0.74,
                            ""pressure"": 1017.31,
                            ""windSpeed"": 8.11,
                            ""windGust"": 10.97,
                            ""windBearing"": 254,
                            ""cloudCover"": 0.09,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 283.6
                        },
                        {
                            ""time"": 1567825200,
                            ""summary"": ""Clear"",
                            ""icon"": ""clear-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 62.88,
                            ""apparentTemperature"": 62.88,
                            ""dewPoint"": 55.68,
                            ""humidity"": 0.77,
                            ""pressure"": 1017.36,
                            ""windSpeed"": 7.52,
                            ""windGust"": 10.62,
                            ""windBearing"": 257,
                            ""cloudCover"": 0.08,
                            ""uvIndex"": 0,
                            ""visibility"": 9.173,
                            ""ozone"": 281.6
                        },
                        {
                            ""time"": 1567828800,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 62.22,
                            ""apparentTemperature"": 62.22,
                            ""dewPoint"": 56.12,
                            ""humidity"": 0.8,
                            ""pressure"": 1017.52,
                            ""windSpeed"": 6.16,
                            ""windGust"": 8.49,
                            ""windBearing"": 262,
                            ""cloudCover"": 0.13,
                            ""uvIndex"": 0,
                            ""visibility"": 9.43,
                            ""ozone"": 280.7
                        },
                        {
                            ""time"": 1567832400,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 61.03,
                            ""apparentTemperature"": 61.03,
                            ""dewPoint"": 56.61,
                            ""humidity"": 0.85,
                            ""pressure"": 1017.9,
                            ""windSpeed"": 6.59,
                            ""windGust"": 9.34,
                            ""windBearing"": 233,
                            ""cloudCover"": 0.54,
                            ""uvIndex"": 0,
                            ""visibility"": 10,
                            ""ozone"": 280.1
                        },
                        {
                            ""time"": 1567836000,
                            ""summary"": ""Mostly Cloudy"",
                            ""icon"": ""partly-cloudy-night"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 60.65,
                            ""apparentTemperature"": 60.65,
                            ""dewPoint"": 56,
                            ""humidity"": 0.85,
                            ""pressure"": 1017.83,
                            ""windSpeed"": 6.6,
                            ""windGust"": 9.3,
                            ""windBearing"": 243,
                            ""cloudCover"": 0.67,
                            ""uvIndex"": 0,
                            ""visibility"": 8.91,
                            ""ozone"": 280.8
                        }
                        ]
                    },
                    ""daily"": {
                        ""data"": [
                        {
                            ""time"": 1567753200,
                            ""summary"": ""Mostly cloudy throughout the day."",
                            ""icon"": ""partly-cloudy-day"",
                            ""sunriseTime"": 1567777528,
                            ""sunsetTime"": 1567823613,
                            ""moonPhase"": 0.28,
                            ""precipIntensity"": 0,
                            ""precipIntensityMax"": 0.0001,
                            ""precipIntensityMaxTime"": 1567832400,
                            ""precipProbability"": 0.04,
                            ""temperatureHigh"": 67.27,
                            ""temperatureHighTime"": 1567814400,
                            ""temperatureLow"": 58.89,
                            ""temperatureLowTime"": 1567868400,
                            ""apparentTemperatureHigh"": 67.27,
                            ""apparentTemperatureHighTime"": 1567814400,
                            ""apparentTemperatureLow"": 58.89,
                            ""apparentTemperatureLowTime"": 1567868400,
                            ""dewPoint"": 56.31,
                            ""humidity"": 0.83,
                            ""pressure"": 1017.96,
                            ""windSpeed"": 6.57,
                            ""windGust"": 12.55,
                            ""windGustTime"": 1567803600,
                            ""windBearing"": 244,
                            ""cloudCover"": 0.49,
                            ""uvIndex"": 8,
                            ""uvIndexTime"": 1567803600,
                            ""visibility"": 9.06,
                            ""ozone"": 282.5,
                            ""temperatureMin"": 58.4,
                            ""temperatureMinTime"": 1567785600,
                            ""temperatureMax"": 67.27,
                            ""temperatureMaxTime"": 1567814400,
                            ""apparentTemperatureMin"": 58.4,
                            ""apparentTemperatureMinTime"": 1567785600,
                            ""apparentTemperatureMax"": 67.27,
                            ""apparentTemperatureMaxTime"": 1567814400
                        }
                        ]
                    },
                    ""flags"": {
                        ""sources"": [
                            ""cmc"",
                        ""gfs"",
                        ""hrrr"",
                        ""icon"",
                        ""isd"",
                        ""madis"",
                        ""nam"",
                        ""sref""
                        ],
                        ""nearest-station"": 1.835,
                        ""units"": ""us""
                    },
                    ""offset"": -7
            }";

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            var eventStoreMock = new Mock<IEventStore>();
            moqEventMetadata.Setup(Id => Id.TenantId).Returns(Guid.NewGuid());
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
            var expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.023434,-84.615494" + "," + now.AddDays(-7).ToUnixTimeSeconds());

            httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(7),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri),
                    ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void Start_Irrigatio_Should_Send_One_Event_To_EventStore()
        {
            CommandHandlerRegistration.RegisterCommandHandler();

            var tenantId = Guid.NewGuid();
            StartIrrigationCommand cmd = new StartIrrigationCommand()
            {
                Zone = Guid.NewGuid(),
                TenantId = tenantId
            };
            moqEventMetadata.Setup(Id => Id.TenantId).Returns(tenantId);

            var zone = new Domain.Zone(cmd.Zone, moqEventStore.Object);
            zone.StartIrrigation(cmd, moqEventMetadata.Object);

            moqEventStore.Verify(m => m.SaveEvents(It.IsAny<CompositeAggregateId>(), It.IsAny<IEnumerable<IEvent>>()), Times.Once);
        }

        private static string GetPath()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return path;
        }
    }
}
