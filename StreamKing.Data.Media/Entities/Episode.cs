using System.ComponentModel.DataAnnotations;

namespace StreamKing.Data.Media
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }
        public int? Number { get; set; }
        public string? Title { get; set; }
        public DateTime? AirDate { get; set; }
        public double? Rating { get; set; }
        public int? VoteCount { get; set; }
        public string? StillPath { get; set; }
    }
}
