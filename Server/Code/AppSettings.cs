namespace Mars {
    public class AppSettings {
        public string APIKey { get; set; }
        public string MarsRoverPhotosUrl { get; set; }
        public string InSightUrl { get; set; }
        public int PhotoPageNumber { get; set; }        
    }
    public class FakeDataPath
    {
        public string CuriosityPath { get; set; }
        public string OpportunityPath { get; set; }
        public string InsightWeatherPath { get; set; }
        public string SpritPath { get; set; }
    }
}