using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace HomeIrrigation.Sprinkler.Service
{
    public class IrrigationCalculator
    {
        private static IConfigurationRoot Configuration;

        public double HowLongToIrrigate(double rainfallInPass7Days, double irrigationInPass7Days)
        {
            double minutesToIrrigate;
            string path = GetPath();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(path))
                .AddJsonFile("appsettings.json", false, true);
            Configuration = builder.Build();

            minutesToIrrigate = 0;
            var totalIrrigationInches = Configuration.GetSection("TotalIrrigationInches").Value;
            if(rainfallInPass7Days + irrigationInPass7Days < int.Parse(totalIrrigationInches))
            {
                var MinutesToIrrigateToGetOneInch = Configuration.GetSection("MinutesToIrrigateToGetOneInch").Value;
                minutesToIrrigate = int.Parse(totalIrrigationInches) * int.Parse(MinutesToIrrigateToGetOneInch);
            }

            return minutesToIrrigate;
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
