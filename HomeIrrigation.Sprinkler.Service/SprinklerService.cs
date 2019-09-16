using System;
using System.Threading;
using System.Configuration;
using HomeIrrigation.Data.DataAccess.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Net.Http;
using HomeIrrigation.Weather.Service;

namespace HomeIrrigation.Sprinkler.Service
{
    public class SprinklerService
    {
        public SprinklerService(ILogger logger, ITimer timer, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _timer = timer;
            string path = GetPath();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(path))
                .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();
            _timer.Elapsed += SchedulerCallback;
            GetInterval();
            ChangeSchedule();
        }

        private readonly ILogger _logger;
        private ITimer _timer;
        private DateTime scheduledTime = DateTime.MinValue;
        private HttpClient _httpClient;
        private static IConfigurationRoot Configuration;

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
