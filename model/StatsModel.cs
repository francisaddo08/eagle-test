using EagleEyeTest.database;

namespace EagleEyeTest.model
{
    public class StatsModel 
    {
        public int movieId { get; set; }
       
        public string title { get; set; }
        public long averageWatchDurationS { get; set; }
        public long watches { get; set; }
       
        public int releaseYear { get; set; }
    }
}
