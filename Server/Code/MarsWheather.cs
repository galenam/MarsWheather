using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mars{

    [JsonConverter(typeof(MarsWeatherRootObjectConverter))]
    public class MarsWeatherRootObject
    {
        public List<MarsWeather> MarsWeather { get; set; }
    }
    public class MarsWeather
    {

        public int Sol { get; set; }
        [JsonPropertyName("First_UTC")]
        public DateTime FirstUTC { get; set; }
        [JsonPropertyName("Last_UTC")]
        public DateTime LastUTC { get; set; }
        [JsonPropertyName("Season")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Season MarsSeason { get; set; }
        [JsonPropertyName("PRE")]
        public DataDescription AtmosphericPressure { get; set; }

        [JsonIgnore]
        public HashSet<string> Photos { get; set; }
        [JsonIgnore]
        public List<RoverInfo> Rovers { get; set; }

    }

    public enum Season {
        winter,
        spring,
        summer,
        autumn
    }

    public class DataDescription{
        [JsonPropertyName("av")]
        public double Average { get; set; }
        [JsonPropertyName("ct")]
        public double TotalCount { get; set; }
        [JsonPropertyName("mn")]
        public double Minimum { get; set; }
        [JsonPropertyName("mx")]
        public double Maximum { get; set; }
    }
    public enum RoverName
    {
        Curiosity,
        Opportunity,
        Spirit
    }

    public class RoverInfo {

        public string Name { get; set; }
        public DateTime LandingDate { get; set; }
        public DateTime LaunchDate { get; set; }
        public string Status { get; set; }
    }
}