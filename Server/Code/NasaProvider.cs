using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;

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
        public async Task<IEnumerable<MarsWeather>> GetAsync()
        {
            List<MarsWeather> weathers = null;
            if (!memory.TryGetValue(key, out weathers))
            {
                try
                {                    
                    var stream = await nasaStream.GetDataAsync();
                    weathers = new List<MarsWeather>(await CustomDeserializer.GetAsync(stream));
                    foreach (var weather in weathers)
                    {
                        weather.Photos = new HashSet<string>();
                        weather.Rovers = new List<RoverInfo>();

                        foreach (var rName in (RoverName[])Enum.GetValues(typeof(RoverName)))
                        {
                            var photoStream = await nasaStream.GetPhotoAsync(rName, weather.FirstUTC);
                            var photoDTO = await JsonSerializer.DeserializeAsync<MarsPhotosDTO>(photoStream);
                            var photos = photoDTO.photos.Select(ph => ph.img_src);
                            weather.Photos.UnionWith(photos);
                            var photoInfo = photoDTO?.photos?.FirstOrDefault();
                            if (photoInfo == null)
                            {
                                continue;
                            }
                            if (photoInfo.rover == null)
                            {
                                continue;
                            }
                            var rInfo = new RoverInfo
                            {
                                LandingDate = photoInfo.rover.landing_date,
                                LaunchDate = photoInfo.rover.launch_date,
                                Status = photoInfo.rover.status,
                                Name = rName.ToString()
                            };
                            weather.Rovers.Add(rInfo);
                        }
                    }

                    memory.Set(key, weathers, DateTimeOffset.Now.AddDays(1));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "No data deserialized");
                }
            }
            return weathers;
        }

    }
}