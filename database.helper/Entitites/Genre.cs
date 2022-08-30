using System.ComponentModel.DataAnnotations;

namespace database.helper.Entitites
{
    public class Genre
    {
        [Key]
        public int InstanceId { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
