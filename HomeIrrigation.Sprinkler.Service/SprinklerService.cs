using System;
using System.Threading;
using System.Configuration;
using HomeIrrigation.Data.DataAccess.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace HomeIrrigation.Sprinkler.Service
{
    public class SprinklerService
    {
        public SprinklerService(ILogger logger)
        {
            _logger = logger;
            string path = GetPath();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(path))
                .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();
            Scheduler = new Timer(new TimerCallback(SchedulerCallback));
            GetInterval();
            ChangeSchedule();
        }

        private readonly ILogger _logger;
        private Timer Scheduler;
        private DateTime scheduledTime = DateTime.MinValue;
        private static IConfigurationRoot Configuration;

        public void RecordRain(RainFall rainFall)
        {

        }

        private void GetInterval ()
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

            _logger.LogInformation("Simple Service scheduled to run after: " + schedule + " {0}");

            //Get the difference in Minutes between the Scheduled and Current Time.
            int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

            //Change the Timer's Due Time.
            Scheduler.Change(dueTime, Timeout.Infinite);    
        }

        private void SchedulerCallback(object e)
        {
            _logger.LogInformation("Scheduler callback: " );
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
