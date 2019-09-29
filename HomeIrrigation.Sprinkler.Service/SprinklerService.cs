using System;
using System.Threading;
using HomeIrrigation.Data.DataAccess.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Net.Http;
using HomeIrrigation.Weather.Service;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HomeIrrigation.ESFramework.Common.Base;
using HomeIrrigation.ESFramework.Common.Interfaces;
using HomeIrrigation.Sprinkler.Service.Domain;

namespace HomeIrrigation.Sprinkler.Service
{
    public class SprinklerService
    {
        public SprinklerService(ILogger logger, ITimer timer, HttpClient httpClient, IEventStore eventStore)
        {
            _logger = logger;
            _httpClient = httpClient;
            _timer = (ITimer)timer;
            _timer.Elapsed += SchedulerCallback;
            _realTimer = new System.Threading.Timer(SchedulerCallback);
            _timer.SetRealTimer(_realTimer);
            EventStore = eventStore;
            string path = GetPath();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(path))
                .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();
            GetInterval();
            ChangeSchedule();
        }

        private readonly ILogger _logger;
        private ITimer _timer;
        private System.Threading.Timer _realTimer;
        private DateTime scheduledTime = DateTime.MinValue;
        private HttpClient _httpClient;
        private static IConfigurationRoot Configuration;
        private IEventStore EventStore;

        public void RecordRain(RainFall rainFall)
        {

        }

        private void GetInterval()
        {
            //Get the Interval in Minutes from AppSettings.
            int intervalMinutes = int.Parse(Configuration.GetSection("IntervalMinutes").Value);

            //Set the Scheduled Time by adding the Interval to Current Time.
            _logger.LogInformation("intervalMinutes: " + intervalMinutes);

            scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
            if (DateTime.Now > scheduledTime)
            {
                //If Scheduled Time is passed set Schedule for the next Interval.
                scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
            }
        }

        private void ChangeSchedule()
        {
            TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
            string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            _logger.LogInformation("Sprinkler Service scheduled to run after: " + schedule);

            //Get the difference in Minutes between the Scheduled and Current Time.
            int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

            //Change the Timer's Due Time.
            _timer.Change(dueTime, Timeout.Infinite);
        }

        private void SchedulerCallback(object sender)
        {
            _logger.LogInformation("Scheduler callback: ");

            var now = DateTimeOffset.Now;
            var weatherService = new WeatherService(_httpClient);
            var s = new IrrigationCalculator();

            var result = weatherService.GetRainfallInPastWeek(double.Parse(Configuration.GetSection("Latitude").Value), double.Parse(Configuration.GetSection("Longitude").Value), now);
            var irrigateFor = s.HowLongToIrrigate(result, 0);
            var eventMetadata = new EventMetadata();
            RunSprinklerCycle(irrigateFor, eventMetadata);
            GetInterval();
            ChangeSchedule();
        }

        private void RunSprinklerCycle(double howLongToIrrigate, IEventMetadata eventMetadata)
        {
            using (StreamReader file = File.OpenText(Path.GetDirectoryName(GetPath()) + @"\appsettings.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o2 = (JObject)JToken.ReadFrom(reader);
                dynamic obj = JsonConvert.DeserializeObject(o2.ToString());
                foreach (var zone in obj["IrrigationZones"])
                {
                    StartIrrigationForZone(Guid.Parse(zone["Number"].Value), howLongToIrrigate, eventMetadata);
                }
            }
        }

        private void StartIrrigationForZone(Guid zoneNumber, double howLongToIrrigate, IEventMetadata eventMetadata)
        {
            Zone zone = new Zone(EventStore, zoneNumber);
            zone.StartIrrigation(howLongToIrrigate, eventMetadata);
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
