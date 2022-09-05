namespace StreamKing.Web.Models
{
    public class WatchEntryRequest
    {
        public string? Tag { get; set; }
        public int? MovieId { get; set; }
        public int? SeriesId { get; set; }
        public int? SeasonId { get; set; }
        public int? EpisodeId { get; set; }
    }
}
