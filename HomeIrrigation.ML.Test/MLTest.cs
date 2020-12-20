using System.Net.Http;
using NUnit.Framework;

namespace HomeIrrigation.ML.Test
{
    public class MLTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadWeatherFile()
        {
            HttpClient httpClient = new HttpClient();

            var data = SprinklerLab.ReadDataForPassXDays();
            Assert.AreEqual(44, data.Count);
        }
    }
}
