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
        public static async Task<IEnumerable<MarsWheather>> GetAsync(Stream stream)
        {
            var result = new List<MarsWheather>();
            using (var document = JsonDocument.Parse(stream))
            {
                var root = document.RootElement;
                var keys = root.GetProperty(fieldName);
                foreach (var key in keys.EnumerateArray())
                {
                    if (root.TryGetProperty(key.GetString(), out var element))
                    {
                        try
                        {
                            var wheather = await JsonSerializer.DeserializeAsync<MarsWheather>(new MemoryStream(Encoding.ASCII.GetBytes(element.GetRawText())));
                            int.TryParse(key.GetString(), out var sol);
                            wheather.Sol = sol;
                            result.Add(wheather);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            return result;
        }
    }
}