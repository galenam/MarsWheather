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

        public async Task<Stream> GetDataAsync()
        {
            try
            {
                var url = string.Format(settings.InSightUrl, settings.APIKey);
                return await client.GetStreamAsync(url);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "No information about wheather at all");
                return Stream.Null;
            }
        }
    }
}