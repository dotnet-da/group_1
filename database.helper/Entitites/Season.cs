using System.ComponentModel.DataAnnotations;

namespace database.helper.Entitites
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
