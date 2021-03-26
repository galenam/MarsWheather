namespace Mars {
    public class AppSettings {
        public string APIKey { get; set; }
        public string MarsRoverPhotosUrl { get; set; }
        public string InSightUrl { get; set; }
    }

    public enum Rover {
        Curiosity,
        Opportunity,
        Spirit
    }
}