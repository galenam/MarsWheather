using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mars
{
    public class NasaStream : INasaStream
    {
        AppSettings settings;
        readonly HttpClient client;
        ILogger<NasaProvider> logger;

        public NasaStream(IOptions<AppSettings> options, HttpClient _client, ILogger<NasaProvider> _logger)
        {
            settings = options.Value;
            client = _client;
            logger = _logger;
        }

        private async Task<Stream> GetStreamAsync(string url)
        {
            try
            {
                return await client.GetStreamAsync(url);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "No information");
                return Stream.Null;
            }

        }

        public async Task<Stream> GetPhotoAsync(RoverName name, int sol)
        {
            var url = string.Format(settings.MarsRoverPhotosUrl, settings.APIKey, name.ToString(), sol, settings.PhotoPageNumber);
            return await GetStreamAsync(url);
        }

        public async Task<Stream> GetDataAsync()
        {
            var url = string.Format(settings.InSightUrl, settings.APIKey);
            return await GetStreamAsync(url);
        }
    }
}