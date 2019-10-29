using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace HomeIrrigation.Weather.Service
{
    public class WeatherService
    {
        public WeatherService(HttpClient client)
        {
            this.client = client;
            string path = GetPath();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(path))
                .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();
            darkSkyKey = Configuration.GetSection("darkskykey").Value;
            weatherUrl = "https://api.darksky.net/forecast/" + darkSkyKey + "/";
        }

        HttpClient client;
        private static IConfigurationRoot Configuration;

        string darkSkyKey;
        string weatherUrl;

        public double GetCurrentTempuratureF(double latitude, double longitude)
        {
            double fahrenheit = 0;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            //client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var query = weatherUrl + latitude.ToString() + "," + longitude.ToString();
            HttpResponseMessage response = client.GetAsync(query).Result;

            Task<string> result = response.Content.ReadAsStringAsync();
            var data = result.Result;
            JObject o = JObject.Parse(data);
            fahrenheit = (double)o["currently"]["temperature"];

            return fahrenheit;
        }

        public double GetRainfallInPastWeek(double latitude, double longitude, DateTimeOffset upToNow)
        {
            double rainfall = 0;
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            for(int days = 0; days < 8; days++)
            {
                var query = weatherUrl + latitude.ToString() + "," + longitude.ToString() + "," + upToNow.AddDays(-days).ToUnixTimeSeconds();

                HttpResponseMessage response = client.GetAsync(query).Result;

                Task<string> result = response.Content.ReadAsStringAsync();
                var data = result.Result;
                JObject o = JObject.Parse(data);
                rainfall += (double)o["daily"]["data"][0]["precipIntensityMax"];
            }

            return rainfall;
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
