namespace StreamKing.Data.Media
{
    public class SeriesEntry : WatchEntry
    {
        public Series Series { get; set; }
        public List<SeasonEntry>? Seasons { get; set; } = new List<SeasonEntry>();
    }
}
