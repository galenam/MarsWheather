using NUnit.Framework;
using Mars;
using Moq;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;

namespace server.test
{
    public class NasaProviderTest
    {
        static string path = "..\\..\\..\\Data\\insight_weather.json";
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestNasaProviderGetAsyncGoodFile()
        {
            var moq = new Mock<INasaStream>();
            var moqLogger = new Mock<ILogger<NasaProvider>>();
            var memory = new MemoryCache(new MemoryCacheOptions { });

            using (var stream = File.OpenRead(path))
            {
                moq.Setup(x => x.GetDataAsync().Result).Returns(stream);
                var options = Options.Create<AppSettings>(new AppSettings());
                var provider = new NasaProvider(options, moq.Object, moqLogger.Object, memory);
                var result = await provider.GetAsync();
                Assert.That(result, Is.Not.EqualTo(null));
                Assert.That(result.Sol, Is.EqualTo(781));
                Assert.That(result.FirstUTC, Is.EqualTo(new DateTime(2021, 2, 5, 16, 28, 36)));
                Assert.That(result.LastUTC, Is.EqualTo(new DateTime(2021, 2, 6, 17, 8, 11)));
                Assert.That(result.MarsSeason, Is.EqualTo(Season.winter));
                Assert.IsNotNull(result.AtmosphericPressure);
                Assert.That(result.AtmosphericPressure, Is.Not.EqualTo(null));
                Assert.That(result.AtmosphericPressure.Average, Is.EqualTo(717.422));
                Assert.That(result.AtmosphericPressure.Maximum, Is.EqualTo(742.7693));
                Assert.That(result.AtmosphericPressure.Minimum, Is.EqualTo(695.3022));
                Assert.That(result.AtmosphericPressure.TotalCount, Is.EqualTo(120387));
            }
        }
    }
}