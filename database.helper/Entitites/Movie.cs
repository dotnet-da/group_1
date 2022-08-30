namespace database.helper.Entitites
{
    public class Movie : Media
    {
        public int? Runtime { get; set; }
        public string? ImdbId { get; set; } = "";
        public float? Rating { get; set; } = 0.0f;
        public int? VoteCount { get; set; } = 0;
    }
}
