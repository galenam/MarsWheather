using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mars
{
    public class NasaProvider : INasaProvider
    {
        static string key = "Sol";
        AppSettings settings;
        INasaStream nasaStream;
        ILogger<NasaProvider> logger;

        IMemoryCache memory;
        public NasaProvider(IOptions<AppSettings> options, INasaStream _nasaStream, ILogger<NasaProvider> _logger, IMemoryCache _memory)
        {
            settings = options.Value;
            nasaStream = _nasaStream;
            logger = _logger;
            memory = _memory;
        }
        public async Task<MarsWheather> GetAsync()
        {
            MarsWheather wheather = null;
            if (!memory.TryGetValue(key, out wheather))
            {
                try
                {                    
                    var stream = await nasaStream.GetDataAsync();
                    var root = await JsonSerializer.DeserializeAsync<MarsWheatherRootObject>(stream, new JsonSerializerOptions { IgnoreNullValues = true });
                    wheather = root?.MarsWheather?[0] ?? null;

                    var photoStream = await nasaStream.GetPhotoAsync(RoverName.Curiosity, wheather.Sol);
                    var photo = await JsonSerializer.DeserializeAsync<MarsPhotosDTO>(photoStream);
                    // todo : transfer DTO object to the part of MarsWheather


                    memory.Set(key, wheather, DateTimeOffset.Now.AddDays(1));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "No data deserialized");
                }
            }
            return wheather;
        }


    }
}