using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mars
{
    internal class MarsWheatherRootObjectConverter : JsonConverter<MarsWheatherRootObject>
    {
        public override MarsWheatherRootObject Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var root = new MarsWheatherRootObject();
            var list = new List<MarsWheather>();        
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
                            var value = JsonSerializer.Deserialize<MarsWheather>(ref reader, options);
                            value.Sol = key;
                            list.Add(value);
                        }
                        break;
                    default: break;
                }
            }
            root.MarsWheather = list;
            return root;
        }

        public override void Write(Utf8JsonWriter writer, MarsWheatherRootObject value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}