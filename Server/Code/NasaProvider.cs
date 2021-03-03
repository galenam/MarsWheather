using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mars
{
    public class NasaProvider : INasaProvider
    {
        static readonly HttpClient client = new HttpClient();
        AppSettings settings;
        INasaStream nasaStream;
        ILogger<NasaProvider> logger;
        public NasaProvider(IOptions<AppSettings> options, INasaStream _nasaStream, ILogger<NasaProvider> _logger)
        {
            settings = options.Value;
            nasaStream = _nasaStream;
            logger = _logger;
        }
        public async Task<MarsWheather> GetAsync()
        {

            try
            {
                var stream = await nasaStream.GetDataAsync();
                var root = await JsonSerializer.DeserializeAsync<MarsWheatherRootObject>(stream, new JsonSerializerOptions { IgnoreNullValues = true });
                return root?.MarsWheather?[0] ?? null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
            }
            return null;
        }
    }
}