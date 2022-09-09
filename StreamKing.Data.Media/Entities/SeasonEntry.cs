namespace StreamKing.Data.Media
{
    public class SeasonEntry : WatchEntry
    {
        public Season Season { get; set; }
        public List<EpisodeEntry>? Episodes { get; set; } = new List<EpisodeEntry>();
    }
}
