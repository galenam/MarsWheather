using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mars
{
    internal class MarsWeatherRootObjectConverter : JsonConverter<MarsWeatherRootObject>
    {
        public override MarsWeatherRootObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var root = new MarsWeatherRootObject();
            var list = new List<MarsWeather>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                    case JsonTokenType.String:
                        var keyStr = reader.GetString();
                        int key;
                        if (!int.TryParse(keyStr, out key))
                        {
                            reader.Skip();
                        }
                        else
                        {
                            var value = JsonSerializer.Deserialize<MarsWeather>(ref reader, options);
                            value.Sol = key;
                            list.Add(value);
                        }
                        break;
                    default: break;
                }
            }
            root.MarsWeather = list;
            return root;
        }

        public override void Write(Utf8JsonWriter writer, MarsWeatherRootObject value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}