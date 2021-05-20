using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Mars
{
    public static class CustomDeserializer
    {
        static string fieldName = "sol_keys";
        public static async Task<IEnumerable<MarsWeather>> GetAsync(Stream stream)
        {
            var result = new List<MarsWeather>();
            using (var document = JsonDocument.Parse(stream))
            {
                var root = document.RootElement;
                var keys = root.GetProperty(fieldName);
                foreach (var key in keys.EnumerateArray())
                {
                    if (root.TryGetProperty(key.GetString(), out var element))
                    {
                        var weather = await JsonSerializer.DeserializeAsync<MarsWeather>(new MemoryStream(Encoding.ASCII.GetBytes(element.GetRawText())));
                        int.TryParse(key.GetString(), out var sol);
                        weather.Sol = sol;
                        result.Add(weather);
                    }
                }
            }
            return result;
        }
    }
}