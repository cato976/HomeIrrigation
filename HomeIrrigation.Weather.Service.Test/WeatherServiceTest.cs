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
    [Parallelizable(ParallelScope.Children)]
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

        //[Test]
        //public void Get_Current_Tempurature_Should_Return_Error()
        //{
            //HttpClient client = new HttpClient();
            //WeatherService weatherService = new WeatherService(client);

            //Assert.Throws<Exception>(() => weatherService.GetCurrentTempuratureF(37.8267, -122.4233));
        //}

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
            string numberJson = ReadTestDataFile(Path.Combine("Data", "weatherData.txt"));
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
            result.ShouldEqual(62.4);
            var darkSkyKey = Configuration.GetSection("darkskykey").Value;
            DateTimeOffset upToDate = DateTimeOffset.UtcNow;
            var expectedUri = new Uri("https://api.open-meteo.com/v1/forecast?latitude=37.8267&longitude=-122.4233&hourly=temperature_2m,relative_humidity_2m,rain&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch&start_date=" + upToDate.AddDays(-1).ToString("yyyy-MM-dd") + "&end_date=" + upToDate.ToString("yyyy-MM-dd"));

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
            var expectedUri = new Uri("https://api.open-meteo.com/v1/forecast?latitude=37.8267&longitude=-122.4233&hourly=temperature_2m,relative_humidity_2m,rain&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch&start_date=" + now.AddDays(-7).ToString("yyyy-MM-dd") + "&end_date=" + now.ToString("yyyy-MM-dd"));
            //var expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-7).ToUnixTimeSeconds());

            httpMessageHandler.Protected().Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri),
                    ItExpr.IsAny<CancellationToken>());
            //expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-6).ToUnixTimeSeconds());
            //httpMessageHandler.Protected().Verify(
            //        "SendAsync",
            //        Times.Exactly(1),
            //        ItExpr.Is<HttpRequestMessage>(req =>
            //            req.Method == HttpMethod.Get
            //            && req.RequestUri == expectedUri),
            //        ItExpr.IsAny<CancellationToken>());
            //expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-5).ToUnixTimeSeconds());
            //httpMessageHandler.Protected().Verify(
            //        "SendAsync",
            //        Times.Exactly(1),
            //        ItExpr.Is<HttpRequestMessage>(req =>
            //            req.Method == HttpMethod.Get
            //            && req.RequestUri == expectedUri),
            //        ItExpr.IsAny<CancellationToken>());
            //expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-4).ToUnixTimeSeconds());
            //httpMessageHandler.Protected().Verify(
            //        "SendAsync",
            //        Times.Exactly(1),
            //        ItExpr.Is<HttpRequestMessage>(req =>
            //            req.Method == HttpMethod.Get
            //            && req.RequestUri == expectedUri),
            //        ItExpr.IsAny<CancellationToken>());
            //expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-3).ToUnixTimeSeconds());
            //httpMessageHandler.Protected().Verify(
            //        "SendAsync",
            //        Times.Exactly(1),
            //        ItExpr.Is<HttpRequestMessage>(req =>
            //            req.Method == HttpMethod.Get
            //            && req.RequestUri == expectedUri),
            //        ItExpr.IsAny<CancellationToken>());
            //expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-2).ToUnixTimeSeconds());
            //httpMessageHandler.Protected().Verify(
            //        "SendAsync",
            //        Times.Exactly(1),
            //        ItExpr.Is<HttpRequestMessage>(req =>
            //            req.Method == HttpMethod.Get
            //            && req.RequestUri == expectedUri),
            //        ItExpr.IsAny<CancellationToken>());
            //expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-1).ToUnixTimeSeconds());
            //httpMessageHandler.Protected().Verify(
            //        "SendAsync",
            //        Times.Exactly(1),
            //        ItExpr.Is<HttpRequestMessage>(req =>
            //            req.Method == HttpMethod.Get
            //            && req.RequestUri == expectedUri),
            //        ItExpr.IsAny<CancellationToken>());
            //expectedUri = new Uri("https://api.darksky.net/forecast/" + darkSkyKey + "/37.8267,-122.4233" + "," + now.AddDays(-0).ToUnixTimeSeconds());
            //httpMessageHandler.Protected().Verify(
            //        "SendAsync",
            //        Times.Exactly(1),
            //        ItExpr.Is<HttpRequestMessage>(req =>
            //            req.Method == HttpMethod.Get
            //            && req.RequestUri == expectedUri),
            //        ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        //[Ignore("Used to get data")]
        public void Get_Rainfall_In_Pass_7_Days_With_Mock_Should_Write_To_File_Weather_Data_In_Pass_50_Days()
        { 
            //Arrange
            HttpClient httpClient = new HttpClient();

            var subjectUnderTest = new WeatherService(httpClient);
            DateTimeOffset now = DateTimeOffset.Now;

            subjectUnderTest.GetDataForPassXDays(34.040148, -84.640035, now, 50);
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
