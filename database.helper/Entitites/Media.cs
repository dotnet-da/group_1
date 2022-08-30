using System.ComponentModel.DataAnnotations;

namespace database.helper.Entitites
{
    public class Media
    {
        [Key]
        public int TmdbId { get; set; }
        public string? Title { get; set; } = "";
        public string? Tagline { get; set; } = "";
        public string? Description { get; set; } = "";
        public DateTime? Release { get; set; } = DateTime.MinValue;
        public string? BackdropURL { get; set; } = "";
        public List<Genre>? Genres { get; set; }
        public List<StreamingInfo>? StreamingInfos { get; set; }
    }
}
