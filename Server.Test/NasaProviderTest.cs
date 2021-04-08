using NUnit.Framework;
using Mars;
using Moq;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace server.test
{
    public class NasaProviderTest
    {
        static string path = "..\\..\\..\\Data\\insight_weather.json";
        static string pathCuriosity = "..\\..\\..\\Data\\curiosity.json";
        static string pathSpirit = "..\\..\\..\\Data\\spirit.json";
        static string pathOpportunity = "..\\..\\..\\Data\\opportunity.json";


        [Test]
        public async Task TestNasaProviderGetAsyncGoodFile()
        {
            var moq = new Mock<INasaStream>();
            var moqLogger = new Mock<ILogger<NasaProvider>>();
            var memory = new MemoryCache(new MemoryCacheOptions { });

            using (var stream = File.OpenRead(path))
            {
                using (var streamCuriosity = File.OpenRead(pathCuriosity))
                {
                    using (var streamOpportunity = File.OpenRead(pathOpportunity))

                    {
                        using (var streamSpirit = File.OpenRead(pathSpirit))

                        {


                            moq.Setup(x => x.GetDataAsync().Result).Returns(stream);
                            moq.Setup(x => x.GetPhotoAsync(RoverName.Curiosity, It.IsAny<DateTime>()).Result).Returns(streamCuriosity);
                            moq.Setup(x => x.GetPhotoAsync(RoverName.Opportunity, It.IsAny<DateTime>()).Result).Returns(streamOpportunity);
                            moq.Setup(x => x.GetPhotoAsync(RoverName.Spirit, It.IsAny<DateTime>()).Result).Returns(streamSpirit);

                            var options = Options.Create<AppSettings>(new AppSettings());
                            var provider = new NasaProvider(options, moq.Object, moqLogger.Object, memory);
                            var results = await provider.GetAsync();
                            Assert.That(results.Count(), Is.EqualTo(7));
                            var result = results.ElementAtOrDefault(0);
                            Assert.That(result, Is.Not.EqualTo(null));
                            Assert.That(result.Sol, Is.EqualTo(781));
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

                            var photos = result.Photos;
                            Assert.That(photos, Is.Not.Null);
                            Assert.That(photos.Count, Is.EqualTo(50));
                            var rovers = result.Rovers;
                            Assert.That(rovers, Is.Not.Null);
                            Assert.That(rovers.Count, Is.EqualTo(3));
                        }
                    }
                }
            }
        }
    }
}

