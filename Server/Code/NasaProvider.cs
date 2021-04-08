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
        public async Task<IEnumerable<MarsWheather>> GetAsync()
        {
            List<MarsWheather> wheathers = null;
            if (!memory.TryGetValue(key, out wheathers))
            {
                try
                {                    
                    var stream = await nasaStream.GetDataAsync();
                    var root = await JsonSerializer.DeserializeAsync<MarsWheatherRootObject>(stream, new JsonSerializerOptions { IgnoreNullValues = true });
                    wheathers = root?.MarsWheather ?? null;
                    foreach (var wheather in wheathers)
                    {
                        wheather.Photos = new HashSet<string>();
                        wheather.Rovers = new List<RoverInfo>();

                        foreach (var rName in (RoverName[])Enum.GetValues(typeof(RoverName)))
                        {
                            var photoStream = await nasaStream.GetPhotoAsync(rName, wheather.FirstUTC);
                            var photoDTO = await JsonSerializer.DeserializeAsync<MarsPhotosDTO>(photoStream);
                            var photos = photoDTO.photos.Select(ph => ph.img_src);
                            wheather.Photos.UnionWith(photos);
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
                            wheather.Rovers.Add(rInfo);
                        }
                    }

                    memory.Set(key, wheathers, DateTimeOffset.Now.AddDays(1));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "No data deserialized");
                }
            }
            return wheathers;
        }

    }
}