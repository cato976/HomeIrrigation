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
            //weatherUrl = "https://api.darksky.net/forecast/" + darkSkyKey + "/";
            weatherUrl = "https://api.open-meteo.com/v1/forecast?";
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
            DateTimeOffset upToNow= DateTimeOffset.UtcNow;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var query = weatherUrl + "latitude=" + latitude.ToString() + "&longitude=" + longitude.ToString() + "&hourly=temperature_2m,relative_humidity_2m,rain&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch&start_date=" + upToNow.AddDays(-1).ToString("yyyy-MM-dd")+ "&end_date=" + upToNow.ToString("yyyy-MM-dd");
            HttpResponseMessage response = client.GetAsync(query).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Not a successfull call");
            }

            Task<string> result = response.Content.ReadAsStringAsync();
            var data = result.Result;
            JObject o = JObject.Parse(data);
            fahrenheit = (double)o["hourly"]["temperature_2m"][0];

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

            var query = weatherUrl + "latitude=" + latitude.ToString() + "&longitude=" + longitude.ToString() + "&hourly=temperature_2m,relative_humidity_2m,rain&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch&start_date=" + upToNow.AddDays(-7).ToString("yyyy-MM-dd") + "&end_date=" + upToNow.ToString("yyyy-MM-dd");

            HttpResponseMessage response = client.GetAsync(query).Result;

            Task<string> result = response.Content.ReadAsStringAsync();
            var data = result.Result;
            JObject o = JObject.Parse(data);
            foreach (var hour in o["hourly"]["rain"])
            {
                rainfall += (double)hour;
            }

            return rainfall;
        }

        public void GetDataForPassXDays(double latitude, double longitude, DateTimeOffset upToNow, int numberOfDays)
        {
            String path = "c:\\temp\\WeatherData.txt";

            var handler = new HttpClientHandler();


            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            if (!File.Exists(path))
            {
                // Create a file to write to.
                File.CreateText(path);
            }

            //var query = weatherUrl + latitude.ToString() + "," + longitude.ToString() + "," + upToNow.AddDays(-days).ToUnixTimeSeconds();
            var query = weatherUrl + "latitude=" + latitude.ToString() + "&longitude=" + longitude.ToString() + "&hourly=temperature_2m,relative_humidity_2m,rain&timezone=America%2FNew_York&wind_speed_unit=mph&temperature_unit=fahrenheit&precipitation_unit=inch&start_date=" + upToNow.AddDays(numberOfDays * -1).ToString("yyyy-MM-dd") + "&end_date=" + upToNow.ToString("yyyy-MM-dd");

            HttpResponseMessage response = client.GetAsync(query).Result;

            Task<string> result = response.Content.ReadAsStringAsync();
            var data = result.Result;
            JObject o = JObject.Parse(data);
            // Append to file
            // This text is added only once to the file.
            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(o);
            }
            //System.Threading.Thread.Sleep(1000);

            // Open the file to read from.
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
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
