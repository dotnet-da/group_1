using System.ComponentModel.DataAnnotations;

namespace StreamKing.Web.Models
{
    public class MovieEntryRequest
    {
        public string? Tag { get; set; }

        [Required]
        public int MovieId{ get; set; }
    }
}
