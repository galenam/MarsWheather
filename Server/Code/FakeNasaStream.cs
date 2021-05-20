using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mars
{
    public class FakeNasaStream : INasaStream
    {
        FakeDataPath settings;
        ILogger<FakeNasaStream> logger;
        public FakeNasaStream(IOptions<FakeDataPath> options, ILogger<FakeNasaStream> _logger)
        {
            settings = options.Value;
            logger = _logger;
        }

        public Task<Stream> GetDataAsync()
        {
            Stream stream = null;
            try
            {
                stream = File.OpenRead(settings.InsightWeatherPath);
            }
            catch (System.Exception ex)
            {
                logger.LogInformation(ex, "File.OpenRead");
            }
            return Task.FromResult(stream);
        }

        public Task<Stream> GetPhotoAsync(RoverName name, DateTime date)
        {
            string fileName = name switch
            {
                RoverName.Curiosity => settings.CuriosityPath,
                RoverName.Opportunity => settings.OpportunityPath,
                _ => settings.SpritPath
            };
            Stream stream = null;
            try
            {
                stream = File.OpenRead(fileName);
            }
            catch (System.Exception ex)
            {
                logger.LogInformation(ex, "file open");
            }
            return Task.FromResult(stream);
        }
    }
}