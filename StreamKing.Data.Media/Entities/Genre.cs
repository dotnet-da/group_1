using System.ComponentModel.DataAnnotations;

namespace StreamKing.Data.Media
{
    public class Genre
    {
        [Key]
        public int InstanceId { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
