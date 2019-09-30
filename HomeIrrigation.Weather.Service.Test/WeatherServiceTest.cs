using NUnit.Framework;
using Moq;
using Moq.Protected;
using Should;
using HomeIrrigation.Weather.Service;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace HomeIrrigation.Weather.Service.Test
{
    public class WeatherServiceTest
    {
        private static IConfigurationRoot Configuration;

        [SetUp]
        public void Setup()
        {
            string path = GetPath();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(path))
                .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();
        }

        [Test]
        public void Get_Current_Tempurature_Should_Return_Current_Tempurature()
        {
            HttpClient client = new HttpClient();
            WeatherService weatherService = new WeatherService(client);

            var tempurature = weatherService.GetCurrentTempuratureF(37.8267, -122.4233);
            Assert.AreNotEqual(0, tempurature);
        }

        [Test]
        public void Get_Current_Tempurature_With_Mock_Should_Return_Current_Tempurature()
        {
            //Arrange
            string numberJson = @"
            {
                ""latitude"": 37.8267,
                    ""longitude"": -122.4233,
                    ""timezone"": ""America/Los_Angeles"",
                    ""currently"": {
                        ""time"": 1566760189,
                        ""summary"": ""Partly Cloudy"",
                        ""icon"": ""partly-cloudy-day"",
                        ""nearestStormDistance"": 5,
                        ""nearestStormBearing"": 168,
                        ""precipIntensity"": 0,
                        ""precipProbability"": 0,
                        ""temperature"": 67.59,
                        ""apparentTemperature"": 67.59,
                        ""dewPoint"": 55.86,
                        ""humidity"": 0.66,
                        ""pressure"": 1016.74,
                        ""windSpeed"": 5.27,
                        ""windGust"": 7.86,
                        ""windBearing"": 249,
                        ""cloudCover"": 0.33,
                        ""uvIndex"": 7,
                        ""visibility"": 8.235,
                        ""ozone"": 280
                    },
                    ""minutely"": {
                        ""summary"": ""Partly cloudy for the hour."",
                        ""icon"": ""partly-cloudy-day"",
                        ""data"": [{
                            ""time"": 1566760140,
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0
                        }, {
                            ""time"": 1566760200,
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0
                        }, {
                            ""time"": 1566760260,
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0
                        }, {
                            ""time"": 1566760320,
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0
                        }, {
                            ""time"": 1566760380,
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0
                        }, {
                            ""time"": 1566760440,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760500,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760560,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760620,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760680,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760740,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760800,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760860,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760920,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566760980,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761040,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761100,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761160,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761220,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761280,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761340,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761400,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761460,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761520,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761580,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761640,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761700,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761760,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761820,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761880,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566761940,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762000,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762060,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762120,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762180,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762240,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762300,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762360,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762420,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762480,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762540,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762600,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762660,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762720,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762780,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762840,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762900,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566762960,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763020,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763080,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763140,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763200,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763260,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763320,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763380,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763440,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763500,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763560,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763620,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763680,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }, {
                            ""time"": 1566763740,
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0
                        }
                        ]
                    },
                    ""hourly"": {
                        ""summary"": ""Partly cloudy throughout the day."",
                        ""icon"": ""partly-cloudy-day"",
                        ""data"": [{
                            ""time"": 1566759600,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 67.26,
                            ""apparentTemperature"": 67.26,
                            ""dewPoint"": 55.81,
                            ""humidity"": 0.67,
                            ""pressure"": 1016.77,
                            ""windSpeed"": 5.01,
                            ""windGust"": 7.56,
                            ""windBearing"": 246,
                            ""cloudCover"": 0.34,
                            ""uvIndex"": 7,
                            ""visibility"": 7.886,
                            ""ozone"": 280
                        }, {
                            ""time"": 1566763200,
                            ""summary"": ""Partly Cloudy"",
                            ""icon"": ""partly-cloudy-day"",
                            ""precipIntensity"": 0,
                            ""precipProbability"": 0,
                            ""temperature"": 69.27,
                            ""apparentTemperature"": 69.27,
                            ""dewPoint"": 56.12,
                            ""humidity"": 0.63,
                            ""pressure"": 1016.57,
                            ""windSpeed"": 6.57,
                            ""windGust"": 9.41,
                            ""windBearing"": 258,
                            ""cloudCover"": 0.31,
                            ""uvIndex"": 8,
                            ""visibility"": 10,
                            ""ozone"": 279.9
                        }, {
                            ""time"": 1566766800,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 70.22,
                                ""apparentTemperature"": 70.22,
                                ""dewPoint"": 58.5,
                                ""humidity"": 0.66,
                                ""pressure"": 1015.88,
                                ""windSpeed"": 8.76,
                                ""windGust"": 11,
                                ""windBearing"": 258,
                                ""cloudCover"": 0.2,
                                ""uvIndex"": 8,
                                ""visibility"": 10,
                                ""ozone"": 279.9
                        }, {
                            ""time"": 1566770400,
                                ""summary"": ""Clear"",
                                ""icon"": ""clear-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 70.66,
                                ""apparentTemperature"": 70.66,
                                ""dewPoint"": 59.74,
                                ""humidity"": 0.68,
                                ""pressure"": 1015.09,
                                ""windSpeed"": 9.69,
                                ""windGust"": 12,
                                ""windBearing"": 256,
                                ""cloudCover"": 0.12,
                                ""uvIndex"": 7,
                                ""visibility"": 10,
                                ""ozone"": 279.8
                        }, {
                            ""time"": 1566774000,
                                ""summary"": ""Clear"",
                                ""icon"": ""clear-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 71.02,
                                ""apparentTemperature"": 71.02,
                                ""dewPoint"": 59.06,
                                ""humidity"": 0.66,
                                ""pressure"": 1014.76,
                                ""windSpeed"": 9.22,
                                ""windGust"": 12.28,
                                ""windBearing"": 255,
                                ""cloudCover"": 0.11,
                                ""uvIndex"": 5,
                                ""visibility"": 10,
                                ""ozone"": 279.7
                        }, {
                            ""time"": 1566777600,
                                ""summary"": ""Clear"",
                                ""icon"": ""clear-day"",
                                ""precipIntensity"": 0.0013,
                                ""precipProbability"": 0.01,
                                ""precipType"": ""rain"",
                                ""temperature"": 70.4,
                                ""apparentTemperature"": 70.4,
                                ""dewPoint"": 57.75,
                                ""humidity"": 0.64,
                                ""pressure"": 1014.31,
                                ""windSpeed"": 8.6,
                                ""windGust"": 12,
                                ""windBearing"": 253,
                                ""cloudCover"": 0.12,
                                ""uvIndex"": 3,
                                ""visibility"": 10,
                                ""ozone"": 279.7
                        }, {
                            ""time"": 1566781200,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0.0019,
                                ""precipProbability"": 0.01,
                                ""precipType"": ""rain"",
                                ""temperature"": 68.97,
                                ""apparentTemperature"": 68.97,
                                ""dewPoint"": 56.7,
                                ""humidity"": 0.65,
                                ""pressure"": 1014.38,
                                ""windSpeed"": 8.02,
                                ""windGust"": 11.02,
                                ""windBearing"": 253,
                                ""cloudCover"": 0.13,
                                ""uvIndex"": 1,
                                ""visibility"": 10,
                                ""ozone"": 279.7
                        }, {
                            ""time"": 1566784800,
                                ""summary"": ""Clear"",
                                ""icon"": ""clear-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 67.01,
                                ""apparentTemperature"": 67.01,
                                ""dewPoint"": 56.17,
                                ""humidity"": 0.68,
                                ""pressure"": 1013.92,
                                ""windSpeed"": 7.41,
                                ""windGust"": 10.18,
                                ""windBearing"": 246,
                                ""cloudCover"": 0.12,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 279.8
                        }, {
                            ""time"": 1566788400,
                                ""summary"": ""Clear"",
                                ""icon"": ""clear-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 64.86,
                                ""apparentTemperature"": 64.86,
                                ""dewPoint"": 54.82,
                                ""humidity"": 0.7,
                                ""pressure"": 1013.97,
                                ""windSpeed"": 6.62,
                                ""windGust"": 9.47,
                                ""windBearing"": 243,
                                ""cloudCover"": 0.12,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 280
                        }, {
                            ""time"": 1566792000,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 63.21,
                                ""apparentTemperature"": 63.21,
                                ""dewPoint"": 54.58,
                                ""humidity"": 0.73,
                                ""pressure"": 1014.15,
                                ""windSpeed"": 6.36,
                                ""windGust"": 7.98,
                                ""windBearing"": 238,
                                ""cloudCover"": 0.14,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 280.5
                        }, {
                            ""time"": 1566795600,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0.0011,
                                ""precipProbability"": 0.01,
                                ""precipType"": ""rain"",
                                ""temperature"": 61.92,
                                ""apparentTemperature"": 61.92,
                                ""dewPoint"": 54.46,
                                ""humidity"": 0.77,
                                ""pressure"": 1014.46,
                                ""windSpeed"": 5.9,
                                ""windGust"": 7.3,
                                ""windBearing"": 239,
                                ""cloudCover"": 0.17,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 281.2
                        }, {
                            ""time"": 1566799200,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0.0012,
                                ""precipProbability"": 0.01,
                                ""precipType"": ""rain"",
                                ""temperature"": 61,
                                ""apparentTemperature"": 61,
                                ""dewPoint"": 54.51,
                                ""humidity"": 0.79,
                                ""pressure"": 1014.46,
                                ""windSpeed"": 5.61,
                                ""windGust"": 6.9,
                                ""windBearing"": 238,
                                ""cloudCover"": 0.2,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 281.7
                        }, {
                            ""time"": 1566802800,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 60.89,
                                ""apparentTemperature"": 60.89,
                                ""dewPoint"": 54.65,
                                ""humidity"": 0.8,
                                ""pressure"": 1013.97,
                                ""windSpeed"": 5.39,
                                ""windGust"": 6.69,
                                ""windBearing"": 236,
                                ""cloudCover"": 0.22,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 281.9
                        }, {
                            ""time"": 1566806400,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 60.78,
                                ""apparentTemperature"": 60.78,
                                ""dewPoint"": 54.75,
                                ""humidity"": 0.81,
                                ""pressure"": 1013.62,
                                ""windSpeed"": 5.12,
                                ""windGust"": 6.27,
                                ""windBearing"": 234,
                                ""cloudCover"": 0.24,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 281.9
                        }, {
                            ""time"": 1566810000,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 59.95,
                                ""apparentTemperature"": 59.95,
                                ""dewPoint"": 55.09,
                                ""humidity"": 0.84,
                                ""pressure"": 1013.54,
                                ""windSpeed"": 4.94,
                                ""windGust"": 5.8,
                                ""windBearing"": 230,
                                ""cloudCover"": 0.27,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 282.1
                        }, {
                            ""time"": 1566813600,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 58.83,
                                ""apparentTemperature"": 58.83,
                                ""dewPoint"": 54.9,
                                ""humidity"": 0.87,
                                ""pressure"": 1013.3,
                                ""windSpeed"": 4.73,
                                ""windGust"": 5.3,
                                ""windBearing"": 227,
                                ""cloudCover"": 0.32,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 282.4
                        }, {
                            ""time"": 1566817200,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 57.83,
                                ""apparentTemperature"": 57.83,
                                ""dewPoint"": 54.67,
                                ""humidity"": 0.89,
                                ""pressure"": 1013.2,
                                ""windSpeed"": 4.48,
                                ""windGust"": 4.65,
                                ""windBearing"": 236,
                                ""cloudCover"": 0.38,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 282.9
                        }, {
                            ""time"": 1566820800,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 57.14,
                                ""apparentTemperature"": 57.14,
                                ""dewPoint"": 54.46,
                                ""humidity"": 0.91,
                                ""pressure"": 1013.15,
                                ""windSpeed"": 4.17,
                                ""windGust"": 4.24,
                                ""windBearing"": 238,
                                ""cloudCover"": 0.45,
                                ""uvIndex"": 0,
                                ""visibility"": 9.881,
                                ""ozone"": 283.4
                        }, {
                            ""time"": 1566824400,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 56.27,
                                ""apparentTemperature"": 56.27,
                                ""dewPoint"": 54.32,
                                ""humidity"": 0.93,
                                ""pressure"": 1013.13,
                                ""windSpeed"": 3.69,
                                ""windGust"": 3.82,
                                ""windBearing"": 237,
                                ""cloudCover"": 0.51,
                                ""uvIndex"": 0,
                                ""visibility"": 9.785,
                                ""ozone"": 284
                        }, {
                            ""time"": 1566828000,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 55.65,
                                ""apparentTemperature"": 55.65,
                                ""dewPoint"": 54.89,
                                ""humidity"": 0.97,
                                ""pressure"": 1013.2,
                                ""windSpeed"": 3.19,
                                ""windGust"": 3.46,
                                ""windBearing"": 229,
                                ""cloudCover"": 0.58,
                                ""uvIndex"": 0,
                                ""visibility"": 9.903,
                                ""ozone"": 284.6
                        }, {
                            ""time"": 1566831600,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 56.39,
                                ""apparentTemperature"": 56.39,
                                ""dewPoint"": 55.2,
                                ""humidity"": 0.96,
                                ""pressure"": 1013.23,
                                ""windSpeed"": 3.01,
                                ""windGust"": 3.38,
                                ""windBearing"": 231,
                                ""cloudCover"": 0.62,
                                ""uvIndex"": 1,
                                ""visibility"": 10,
                                ""ozone"": 284.9
                        }, {
                            ""time"": 1566835200,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 59.05,
                                ""apparentTemperature"": 59.05,
                                ""dewPoint"": 55.41,
                                ""humidity"": 0.88,
                                ""pressure"": 1013.49,
                                ""windSpeed"": 3.37,
                                ""windGust"": 3.71,
                                ""windBearing"": 234,
                                ""cloudCover"": 0.57,
                                ""uvIndex"": 2,
                                ""visibility"": 10,
                                ""ozone"": 284.7
                        }, {
                            ""time"": 1566838800,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 62.3,
                                ""apparentTemperature"": 62.3,
                                ""dewPoint"": 56.05,
                                ""humidity"": 0.8,
                                ""pressure"": 1013.58,
                                ""windSpeed"": 4.08,
                                ""windGust"": 4.34,
                                ""windBearing"": 235,
                                ""cloudCover"": 0.48,
                                ""uvIndex"": 4,
                                ""visibility"": 10,
                                ""ozone"": 284.2
                        }, {
                            ""time"": 1566842400,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 65.14,
                                ""apparentTemperature"": 65.14,
                                ""dewPoint"": 57.65,
                                ""humidity"": 0.77,
                                ""pressure"": 1013.51,
                                ""windSpeed"": 4.94,
                                ""windGust"": 5.26,
                                ""windBearing"": 235,
                                ""cloudCover"": 0.39,
                                ""uvIndex"": 5,
                                ""visibility"": 10,
                                ""ozone"": 283.7
                        }, {
                            ""time"": 1566846000,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 67.51,
                                ""apparentTemperature"": 67.51,
                                ""dewPoint"": 58.14,
                                ""humidity"": 0.72,
                                ""pressure"": 1013.15,
                                ""windSpeed"": 5.96,
                                ""windGust"": 6.61,
                                ""windBearing"": 234,
                                ""cloudCover"": 0.33,
                                ""uvIndex"": 7,
                                ""visibility"": 10,
                                ""ozone"": 283.2
                        }, {
                            ""time"": 1566849600,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 69.99,
                                ""apparentTemperature"": 69.99,
                                ""dewPoint"": 58.85,
                                ""humidity"": 0.68,
                                ""pressure"": 1012.43,
                                ""windSpeed"": 7.14,
                                ""windGust"": 8.25,
                                ""windBearing"": 243,
                                ""cloudCover"": 0.27,
                                ""uvIndex"": 8,
                                ""visibility"": 10,
                                ""ozone"": 282.9
                        }, {
                            ""time"": 1566853200,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 71.52,
                                ""apparentTemperature"": 71.52,
                                ""dewPoint"": 59.43,
                                ""humidity"": 0.66,
                                ""pressure"": 1011.84,
                                ""windSpeed"": 8.15,
                                ""windGust"": 9.68,
                                ""windBearing"": 242,
                                ""cloudCover"": 0.24,
                                ""uvIndex"": 8,
                                ""visibility"": 10,
                                ""ozone"": 282.7
                        }, {
                            ""time"": 1566856800,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 71.84,
                                ""apparentTemperature"": 71.84,
                                ""dewPoint"": 59.39,
                                ""humidity"": 0.65,
                                ""pressure"": 1011.36,
                                ""windSpeed"": 9.01,
                                ""windGust"": 10.92,
                                ""windBearing"": 243,
                                ""cloudCover"": 0.26,
                                ""uvIndex"": 7,
                                ""visibility"": 10,
                                ""ozone"": 282.9
                        }, {
                            ""time"": 1566860400,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 71.36,
                                ""apparentTemperature"": 71.36,
                                ""dewPoint"": 58.71,
                                ""humidity"": 0.64,
                                ""pressure"": 1011.21,
                                ""windSpeed"": 9.72,
                                ""windGust"": 11.95,
                                ""windBearing"": 244,
                                ""cloudCover"": 0.3,
                                ""uvIndex"": 5,
                                ""visibility"": 10,
                                ""ozone"": 283.5
                        }, {
                            ""time"": 1566864000,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 70.03,
                                ""apparentTemperature"": 70.03,
                                ""dewPoint"": 57.54,
                                ""humidity"": 0.65,
                                ""pressure"": 1010.97,
                                ""windSpeed"": 9.96,
                                ""windGust"": 12.42,
                                ""windBearing"": 245,
                                ""cloudCover"": 0.36,
                                ""uvIndex"": 3,
                                ""visibility"": 10,
                                ""ozone"": 283.9
                        }, {
                            ""time"": 1566867600,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 68.15,
                                ""apparentTemperature"": 68.15,
                                ""dewPoint"": 56.49,
                                ""humidity"": 0.66,
                                ""pressure"": 1010.74,
                                ""windSpeed"": 9.48,
                                ""windGust"": 11.93,
                                ""windBearing"": 245,
                                ""cloudCover"": 0.42,
                                ""uvIndex"": 1,
                                ""visibility"": 10,
                                ""ozone"": 284.4
                        }, {
                            ""time"": 1566871200,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 65.79,
                                ""apparentTemperature"": 65.79,
                                ""dewPoint"": 55.5,
                                ""humidity"": 0.69,
                                ""pressure"": 1010.45,
                                ""windSpeed"": 8.53,
                                ""windGust"": 10.86,
                                ""windBearing"": 245,
                                ""cloudCover"": 0.49,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 284.7
                        }, {
                            ""time"": 1566874800,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 63.57,
                                ""apparentTemperature"": 63.57,
                                ""dewPoint"": 55.35,
                                ""humidity"": 0.75,
                                ""pressure"": 1010.46,
                                ""windSpeed"": 7.63,
                                ""windGust"": 9.74,
                                ""windBearing"": 244,
                                ""cloudCover"": 0.54,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 285.1
                        }, {
                            ""time"": 1566878400,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 62.23,
                                ""apparentTemperature"": 62.23,
                                ""dewPoint"": 55.43,
                                ""humidity"": 0.78,
                                ""pressure"": 1010.8,
                                ""windSpeed"": 6.93,
                                ""windGust"": 8.65,
                                ""windBearing"": 242,
                                ""cloudCover"": 0.54,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 285.4
                        }, {
                            ""time"": 1566882000,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 61.44,
                                ""apparentTemperature"": 61.44,
                                ""dewPoint"": 55.54,
                                ""humidity"": 0.81,
                                ""pressure"": 1011.24,
                                ""windSpeed"": 6.27,
                                ""windGust"": 7.49,
                                ""windBearing"": 231,
                                ""cloudCover"": 0.54,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 285.6
                        }, {
                            ""time"": 1566885600,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 60.85,
                                ""apparentTemperature"": 60.85,
                                ""dewPoint"": 55.71,
                                ""humidity"": 0.83,
                                ""pressure"": 1011.59,
                                ""windSpeed"": 5.75,
                                ""windGust"": 6.6,
                                ""windBearing"": 230,
                                ""cloudCover"": 0.55,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 285.8
                        }, {
                            ""time"": 1566889200,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 60.83,
                                ""apparentTemperature"": 60.83,
                                ""dewPoint"": 55.91,
                                ""humidity"": 0.84,
                                ""pressure"": 1011.37,
                                ""windSpeed"": 5.47,
                                ""windGust"": 6.13,
                                ""windBearing"": 232,
                                ""cloudCover"": 0.59,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 286.1
                        }, {
                            ""time"": 1566892800,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 61.06,
                                ""apparentTemperature"": 61.06,
                                ""dewPoint"": 56.03,
                                ""humidity"": 0.84,
                                ""pressure"": 1010.9,
                                ""windSpeed"": 5.36,
                                ""windGust"": 5.87,
                                ""windBearing"": 233,
                                ""cloudCover"": 0.65,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 286.2
                        }, {
                            ""time"": 1566896400,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 60.65,
                                ""apparentTemperature"": 60.65,
                                ""dewPoint"": 55.97,
                                ""humidity"": 0.85,
                                ""pressure"": 1010.5,
                                ""windSpeed"": 5.2,
                                ""windGust"": 5.63,
                                ""windBearing"": 233,
                                ""cloudCover"": 0.71,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 286.4
                        }, {
                            ""time"": 1566900000,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 59.65,
                                ""apparentTemperature"": 59.65,
                                ""dewPoint"": 55.81,
                                ""humidity"": 0.87,
                                ""pressure"": 1010.28,
                                ""windSpeed"": 4.93,
                                ""windGust"": 5.37,
                                ""windBearing"": 229,
                                ""cloudCover"": 0.74,
                                ""uvIndex"": 0,
                                ""visibility"": 10,
                                ""ozone"": 286.6
                        }, {
                            ""time"": 1566903600,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 58.66,
                                ""apparentTemperature"": 58.66,
                                ""dewPoint"": 55.6,
                                ""humidity"": 0.9,
                                ""pressure"": 1010.24,
                                ""windSpeed"": 4.63,
                                ""windGust"": 5.14,
                                ""windBearing"": 236,
                                ""cloudCover"": 0.76,
                                ""uvIndex"": 0,
                                ""visibility"": 8.913,
                                ""ozone"": 286.9
                        }, {
                            ""time"": 1566907200,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 57.86,
                                ""apparentTemperature"": 57.86,
                                ""dewPoint"": 55.39,
                                ""humidity"": 0.91,
                                ""pressure"": 1010.17,
                                ""windSpeed"": 4.33,
                                ""windGust"": 4.88,
                                ""windBearing"": 234,
                                ""cloudCover"": 0.76,
                                ""uvIndex"": 0,
                                ""visibility"": 8.226,
                                ""ozone"": 287.3
                        }, {
                            ""time"": 1566910800,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-night"",
                                ""precipIntensity"": 0.0003,
                                ""precipProbability"": 0.01,
                                ""precipType"": ""rain"",
                                ""temperature"": 57.14,
                                ""apparentTemperature"": 57.14,
                                ""dewPoint"": 55.29,
                                ""humidity"": 0.94,
                                ""pressure"": 1010.15,
                                ""windSpeed"": 3.97,
                                ""windGust"": 4.51,
                                ""windBearing"": 230,
                                ""cloudCover"": 0.72,
                                ""uvIndex"": 0,
                                ""visibility"": 8.594,
                                ""ozone"": 287.7
                        }, {
                            ""time"": 1566914400,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 56.56,
                                ""apparentTemperature"": 56.56,
                                ""dewPoint"": 56.03,
                                ""humidity"": 0.98,
                                ""pressure"": 1010.27,
                                ""windSpeed"": 3.64,
                                ""windGust"": 4.11,
                                ""windBearing"": 235,
                                ""cloudCover"": 0.67,
                                ""uvIndex"": 0,
                                ""visibility"": 9.394,
                                ""ozone"": 288.4
                        }, {
                            ""time"": 1566918000,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 57.24,
                                ""apparentTemperature"": 57.24,
                                ""dewPoint"": 56.22,
                                ""humidity"": 0.96,
                                ""pressure"": 1010.39,
                                ""windSpeed"": 3.58,
                                ""windGust"": 4.02,
                                ""windBearing"": 235,
                                ""cloudCover"": 0.63,
                                ""uvIndex"": 1,
                                ""visibility"": 10,
                                ""ozone"": 288.9
                        }, {
                            ""time"": 1566921600,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 59.48,
                                ""apparentTemperature"": 59.48,
                                ""dewPoint"": 56.47,
                                ""humidity"": 0.9,
                                ""pressure"": 1010.67,
                                ""windSpeed"": 3.88,
                                ""windGust"": 4.28,
                                ""windBearing"": 235,
                                ""cloudCover"": 0.59,
                                ""uvIndex"": 2,
                                ""visibility"": 10,
                                ""ozone"": 289.3
                        }, {
                            ""time"": 1566925200,
                                ""summary"": ""Mostly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 62.6,
                                ""apparentTemperature"": 62.6,
                                ""dewPoint"": 57.06,
                                ""humidity"": 0.82,
                                ""pressure"": 1010.87,
                                ""windSpeed"": 4.46,
                                ""windGust"": 4.8,
                                ""windBearing"": 228,
                                ""cloudCover"": 0.51,
                                ""uvIndex"": 3,
                                ""visibility"": 10,
                                ""ozone"": 289.7
                        }, {
                            ""time"": 1566928800,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 65.26,
                                ""apparentTemperature"": 65.26,
                                ""dewPoint"": 58.51,
                                ""humidity"": 0.79,
                                ""pressure"": 1010.79,
                                ""windSpeed"": 5.29,
                                ""windGust"": 5.69,
                                ""windBearing"": 234,
                                ""cloudCover"": 0.44,
                                ""uvIndex"": 5,
                                ""visibility"": 10,
                                ""ozone"": 289.8
                        }, {
                            ""time"": 1566932400,
                                ""summary"": ""Partly Cloudy"",
                                ""icon"": ""partly-cloudy-day"",
                                ""precipIntensity"": 0,
                                ""precipProbability"": 0,
                                ""temperature"": 67.27,
                                ""apparentTemperature"": 67.27,
                                ""dewPoint"": 58.89,
                                ""humidity"": 0.75,
                                ""pressure"": 1010.78,
                                ""windSpeed"": 6.58,
                                ""windGust"": 7.13,
                                ""windBearing"": 238,
                                ""cloudCover"": 0.36,
                                ""uvIndex"": 7,
                                ""visibility"": 10,
                                ""ozone"": 289.8
                        }
                        ]
                    },
                    ""daily"": {
                        ""summary"": ""No precipitation throughout the week, with high temperatures falling to 69°F on Thursday."",
                        ""icon"": ""clear-day"",
                        ""data"": [{
                            ""time"": 1566716400,
                            ""summary"": ""Partly cloudy throughout the day."",
                            ""icon"": ""partly-cloudy-day"",
                            ""sunriseTime"": 1566740119,
                            ""sunsetTime"": 1566787873,
                            ""moonPhase"": 0.84,
                            ""precipIntensity"": 0.0003,
                            ""precipIntensityMax"": 0.0019,
                            ""precipIntensityMaxTime"": 1566781200,
                            ""precipProbability"": 0.04,
                            ""precipType"": ""rain"",
                            ""temperatureHigh"": 71.02,
                            ""temperatureHighTime"": 1566774000,
                            ""temperatureLow"": 55.65,
                            ""temperatureLowTime"": 1566828000,
                            ""apparentTemperatureHigh"": 71.02,
                            ""apparentTemperatureHighTime"": 1566774000,
                            ""apparentTemperatureLow"": 55.65,
                            ""apparentTemperatureLowTime"": 1566828000,
                            ""dewPoint"": 55.01,
                            ""humidity"": 0.73,
                            ""pressure"": 1015.15,
                            ""windSpeed"": 5.43,
                            ""windGust"": 12.28,
                            ""windGustTime"": 1566774000,
                            ""windBearing"": 248,
                            ""cloudCover"": 0.29,
                            ""uvIndex"": 8,
                            ""uvIndexTime"": 1566766800,
                            ""visibility"": 8.981,
                            ""ozone"": 280.3,
                            ""temperatureMin"": 58.03,
                            ""temperatureMinTime"": 1566741600,
                            ""temperatureMax"": 71.02,
                            ""temperatureMaxTime"": 1566774000,
                            ""apparentTemperatureMin"": 58.03,
                            ""apparentTemperatureMinTime"": 1566741600,
                            ""apparentTemperatureMax"": 71.02,
                            ""apparentTemperatureMaxTime"": 1566774000
                        }, {
                            ""time"": 1566802800,
                                ""summary"": ""Partly cloudy throughout the day."",
                                ""icon"": ""partly-cloudy-day"",
                                ""sunriseTime"": 1566826571,
                                ""sunsetTime"": 1566874188,
                                ""moonPhase"": 0.87,
                                ""precipIntensity"": 0.0001,
                                ""precipIntensityMax"": 0.0003,
                                ""precipIntensityMaxTime"": 1566810000,
                                ""precipProbability"": 0.02,
                                ""precipType"": ""rain"",
                                ""temperatureHigh"": 71.84,
                                ""temperatureHighTime"": 1566856800,
                                ""temperatureLow"": 56.56,
                                ""temperatureLowTime"": 1566914400,
                                ""apparentTemperatureHigh"": 71.84,
                                ""apparentTemperatureHighTime"": 1566856800,
                                ""apparentTemperatureLow"": 56.56,
                                ""apparentTemperatureLowTime"": 1566914400,
                                ""dewPoint"": 56.16,
                                ""humidity"": 0.79,
                                ""pressure"": 1012.34,
                                ""windSpeed"": 6.04,
                                ""windGust"": 12.42,
                                ""windGustTime"": 1566864000,
                                ""windBearing"": 239,
                                ""cloudCover"": 0.42,
                                ""uvIndex"": 8,
                                ""uvIndexTime"": 1566849600,
                                ""visibility"": 9.983,
                                ""ozone"": 283.9,
                                ""temperatureMin"": 55.65,
                                ""temperatureMinTime"": 1566828000,
                                ""temperatureMax"": 71.84,
                                ""temperatureMaxTime"": 1566856800,
                                ""apparentTemperatureMin"": 55.65,
                                ""apparentTemperatureMinTime"": 1566828000,
                                ""apparentTemperatureMax"": 71.84,
                                ""apparentTemperatureMaxTime"": 1566856800
                        }, {
                            ""time"": 1566889200,
                                ""summary"": ""Partly cloudy throughout the day."",
                                ""icon"": ""partly-cloudy-day"",
                                ""sunriseTime"": 1566913022,
                                ""sunsetTime"": 1566960503,
                                ""moonPhase"": 0.91,
                                ""precipIntensity"": 0.0005,
                                ""precipIntensityMax"": 0.0024,
                                ""precipIntensityMaxTime"": 1566961200,
                                ""precipProbability"": 0.08,
                                ""precipType"": ""rain"",
                                ""temperatureHigh"": 70.99,
                                ""temperatureHighTime"": 1566943200,
                                ""temperatureLow"": 58.09,
                                ""temperatureLowTime"": 1567000800,
                                ""apparentTemperatureHigh"": 70.99,
                                ""apparentTemperatureHighTime"": 1566943200,
                                ""apparentTemperatureLow"": 58.3,
                                ""apparentTemperatureLowTime"": 1566997200,
                                ""dewPoint"": 56.87,
                                ""humidity"": 0.8,
                                ""pressure"": 1010.2,
                                ""windSpeed"": 6.44,
                                ""windGust"": 13.79,
                                ""windGustTime"": 1566950400,
                                ""windBearing"": 239,
                                ""cloudCover"": 0.43,
                                ""uvIndex"": 8,
                                ""uvIndexTime"": 1566936000,
                                ""visibility"": 9.805,
                                ""ozone"": 289.5,
                                ""temperatureMin"": 56.56,
                                ""temperatureMinTime"": 1566914400,
                                ""temperatureMax"": 70.99,
                                ""temperatureMaxTime"": 1566943200,
                                ""apparentTemperatureMin"": 56.56,
                                ""apparentTemperatureMinTime"": 1566914400,
                                ""apparentTemperatureMax"": 70.99,
                                ""apparentTemperatureMaxTime"": 1566943200
                        }, {
                            ""time"": 1566975600,
                                ""summary"": ""Partly cloudy throughout the day."",
                                ""icon"": ""partly-cloudy-day"",
                                ""sunriseTime"": 1566999473,
                                ""sunsetTime"": 1567046817,
                                ""moonPhase"": 0.95,
                                ""precipIntensity"": 0.0002,
                                ""precipIntensityMax"": 0.0006,
                                ""precipIntensityMaxTime"": 1566993600,
                                ""precipProbability"": 0.1,
                                ""precipType"": ""rain"",
                                ""temperatureHigh"": 69.21,
                                ""temperatureHighTime"": 1567026000,
                                ""temperatureLow"": 62.17,
                                ""temperatureLowTime"": 1567090800,
                                ""apparentTemperatureHigh"": 69.34,
                                ""apparentTemperatureHighTime"": 1567026000,
                                ""apparentTemperatureLow"": 62.59,
                                ""apparentTemperatureLowTime"": 1567090800,
                                ""dewPoint"": 58.67,
                                ""humidity"": 0.84,
                                ""pressure"": 1012.31,
                                ""windSpeed"": 7.83,
                                ""windGust"": 15.72,
                                ""windGustTime"": 1567040400,
                                ""windBearing"": 241,
                                ""cloudCover"": 0.41,
                                ""uvIndex"": 7,
                                ""uvIndexTime"": 1567026000,
                                ""visibility"": 9.993,
                                ""ozone"": 289.8,
                                ""temperatureMin"": 58.09,
                                ""temperatureMinTime"": 1567000800,
                                ""temperatureMax"": 69.21,
                                ""temperatureMaxTime"": 1567026000,
                                ""apparentTemperatureMin"": 58.3,
                                ""apparentTemperatureMinTime"": 1566997200,
                                ""apparentTemperatureMax"": 69.34,
                                ""apparentTemperatureMaxTime"": 1567026000
                        }, {
                            ""time"": 1567062000,
                                ""summary"": ""Clear throughout the day."",
                                ""icon"": ""clear-day"",
                                ""sunriseTime"": 1567085924,
                                ""sunsetTime"": 1567133130,
                                ""moonPhase"": 0.99,
                                ""precipIntensity"": 0.0002,
                                ""precipIntensityMax"": 0.0008,
                                ""precipIntensityMaxTime"": 1567076400,
                                ""precipProbability"": 0.03,
                                ""precipType"": ""rain"",
                                ""temperatureHigh"": 69.06,
                                ""temperatureHighTime"": 1567112400,
                                ""temperatureLow"": 60.94,
                                ""temperatureLowTime"": 1567173600,
                                ""apparentTemperatureHigh"": 69.23,
                                ""apparentTemperatureHighTime"": 1567112400,
                                ""apparentTemperatureLow"": 61.03,
                                ""apparentTemperatureLowTime"": 1567173600,
                                ""dewPoint"": 59.6,
                                ""humidity"": 0.83,
                                ""pressure"": 1015.97,
                                ""windSpeed"": 8.45,
                                ""windGust"": 16.47,
                                ""windGustTime"": 1567123200,
                                ""windBearing"": 253,
                                ""cloudCover"": 0.02,
                                ""uvIndex"": 9,
                                ""uvIndexTime"": 1567108800,
                                ""visibility"": 10,
                                ""ozone"": 284.1,
                                ""temperatureMin"": 62.17,
                                ""temperatureMinTime"": 1567090800,
                                ""temperatureMax"": 69.06,
                                ""temperatureMaxTime"": 1567112400,
                                ""apparentTemperatureMin"": 62.59,
                                ""apparentTemperatureMinTime"": 1567090800,
                                ""apparentTemperatureMax"": 69.23,
                                ""apparentTemperatureMaxTime"": 1567112400
                        }, {
                            ""time"": 1567148400,
                                ""summary"": ""Partly cloudy throughout the day."",
                                ""icon"": ""clear-day"",
                                ""sunriseTime"": 1567172375,
                                ""sunsetTime"": 1567219442,
                                ""moonPhase"": 0.03,
                                ""precipIntensity"": 0,
                                ""precipIntensityMax"": 0,
                                ""precipIntensityMaxTime"": 1567159200,
                                ""precipProbability"": 0,
                                ""temperatureHigh"": 69.58,
                                ""temperatureHighTime"": 1567202400,
                                ""temperatureLow"": 61.01,
                                ""temperatureLowTime"": 1567260000,
                                ""apparentTemperatureHigh"": 69.58,
                                ""apparentTemperatureHighTime"": 1567202400,
                                ""apparentTemperatureLow"": 61.08,
                                ""apparentTemperatureLowTime"": 1567260000,
                                ""dewPoint"": 57.88,
                                ""humidity"": 0.79,
                                ""pressure"": 1016.56,
                                ""windSpeed"": 8.49,
                                ""windGust"": 17.67,
                                ""windGustTime"": 1567209600,
                                ""windBearing"": 263,
                                ""cloudCover"": 0.07,
                                ""uvIndex"": 9,
                                ""uvIndexTime"": 1567195200,
                                ""visibility"": 10,
                                ""ozone"": 282.7,
                                ""temperatureMin"": 60.94,
                                ""temperatureMinTime"": 1567173600,
                                ""temperatureMax"": 69.58,
                                ""temperatureMaxTime"": 1567202400,
                                ""apparentTemperatureMin"": 61.03,
                                ""apparentTemperatureMinTime"": 1567173600,
                                ""apparentTemperatureMax"": 69.58,
                                ""apparentTemperatureMaxTime"": 1567202400
                        }, {
                            ""time"": 1567234800,
                                ""summary"": ""Partly cloudy throughout the day."",
                                ""icon"": ""partly-cloudy-day"",
                                ""sunriseTime"": 1567258826,
                                ""sunsetTime"": 1567305754,
                                ""moonPhase"": 0.07,
                                ""precipIntensity"": 0,
                                ""precipIntensityMax"": 0,
                                ""precipIntensityMaxTime"": 1567274400,
                                ""precipProbability"": 0,
                                ""temperatureHigh"": 71.51,
                                ""temperatureHighTime"": 1567292400,
                                ""temperatureLow"": 60.27,
                                ""temperatureLowTime"": 1567346400,
                                ""apparentTemperatureHigh"": 71.55,
                                ""apparentTemperatureHighTime"": 1567292400,
                                ""apparentTemperatureLow"": 60.35,
                                ""apparentTemperatureLowTime"": 1567346400,
                                ""dewPoint"": 58.48,
                                ""humidity"": 0.79,
                                ""pressure"": 1015.13,
                                ""windSpeed"": 7.94,
                                ""windGust"": 16.68,
                                ""windGustTime"": 1567296000,
                                ""windBearing"": 262,
                                ""cloudCover"": 0.2,
                                ""uvIndex"": 7,
                                ""uvIndexTime"": 1567281600,
                                ""visibility"": 10,
                                ""ozone"": 284.9,
                                ""temperatureMin"": 61.01,
                                ""temperatureMinTime"": 1567260000,
                                ""temperatureMax"": 71.51,
                                ""temperatureMaxTime"": 1567292400,
                                ""apparentTemperatureMin"": 61.08,
                                ""apparentTemperatureMinTime"": 1567260000,
                                ""apparentTemperatureMax"": 71.55,
                                ""apparentTemperatureMaxTime"": 1567292400
                        }, {
                            ""time"": 1567321200,
                                ""summary"": ""Partly cloudy throughout the day."",
                                ""icon"": ""partly-cloudy-day"",
                                ""sunriseTime"": 1567345277,
                                ""sunsetTime"": 1567392065,
                                ""moonPhase"": 0.11,
                                ""precipIntensity"": 0.0001,
                                ""precipIntensityMax"": 0.0004,
                                ""precipIntensityMaxTime"": 1567328400,
                                ""precipProbability"": 0.01,
                                ""precipType"": ""rain"",
                                ""temperatureHigh"": 71.2,
                                ""temperatureHighTime"": 1567375200,
                                ""temperatureLow"": 60.96,
                                ""temperatureLowTime"": 1567425600,
                                ""apparentTemperatureHigh"": 71.2,
                                ""apparentTemperatureHighTime"": 1567375200,
                                ""apparentTemperatureLow"": 60.96,
                                ""apparentTemperatureLowTime"": 1567425600,
                                ""dewPoint"": 57.52,
                                ""humidity"": 0.77,
                                ""pressure"": 1014.29,
                                ""windSpeed"": 7.84,
                                ""windGust"": 13.66,
                                ""windGustTime"": 1567382400,
                                ""windBearing"": 247,
                                ""cloudCover"": 0.15,
                                ""uvIndex"": 8,
                                ""uvIndexTime"": 1567368000,
                                ""visibility"": 10,
                                ""ozone"": 283.4,
                                ""temperatureMin"": 60.27,
                                ""temperatureMinTime"": 1567346400,
                                ""temperatureMax"": 71.2,
                                ""temperatureMaxTime"": 1567375200,
                                ""apparentTemperatureMin"": 60.35,
                                ""apparentTemperatureMinTime"": 1567346400,
                                ""apparentTemperatureMax"": 71.2,
                                ""apparentTemperatureMaxTime"": 1567375200
                        }
                        ]
                    },
                    ""flags"": {
                        ""sources"": [""nwspa"", ""cmc"", ""gfs"", ""hrrr"", ""icon"", ""isd"", ""madis"", ""nam"", ""sref"", ""darksky"", ""nearest-precip""],
                        ""nearest-station"": 1.835,
                        ""units"": ""us""
                    },
                    ""offset"": -7
            }

            ";

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
            var result = subjectUnderTest.GetCurrentTempuratureF(37.8267, -122.4233);
            result.ShouldNotBeSameAs(0);
            result.ShouldEqual(67.59);
            var darkSkyKey = Configuration.GetSection("darkskykey").Value;
            var expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233");

            httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri),
                    ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void Get_Rainfall_In_Pass_7_Days_Should_Get_Rainfall_In_Pass_7_Days()
        {
            HttpClient client = new HttpClient();
            WeatherService weatherService = new WeatherService(client);

            var rainfall = weatherService.GetRainfallInPastWeek(37.8267, -122.4233, DateTimeOffset.Now);
            rainfall.ShouldNotEqual(0);
        }


        [Test]
        public void Get_Rainfall_In_Pass_7_Days_With_Mock_Should_Return_Rainfall_In_Pass_7_Days()
        {
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
