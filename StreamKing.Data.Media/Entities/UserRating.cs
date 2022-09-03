using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamKing.Data.Media
{
    public class UserRating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Rating { get; set; }
        public string? Review { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime? LastUpdated { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
