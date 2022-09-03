using System.ComponentModel.DataAnnotations;

namespace StreamKing.Data.Media
{
    public class Season
    {
        [Key]
        public int Id { get; set; }
        public int? Number { get; set; }
        public string? Description { get; set; }
        public List<Episode>? Episodes { get; set; }
    }
}
