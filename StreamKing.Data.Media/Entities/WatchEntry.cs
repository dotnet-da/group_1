using System.ComponentModel.DataAnnotations;

namespace StreamKing.Data.Media
{
    public abstract class WatchEntry
    {
        [Key]
        public int Id { get; set; }

        public string? Tag { get; set; } = null;

        public DateTime? Created { get; set; } = DateTime.Now.ToUniversalTime();
        public UserRating? UserRating { get; set; }

    }
}
