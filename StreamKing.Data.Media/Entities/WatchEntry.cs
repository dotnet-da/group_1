using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamKing.Data.Media
{
    public abstract class WatchEntry
    {
        [Key]
        public int Id { get; set; }

        public string? Tag { get; set; } = null;

        public DateTime? Created { get; set; } = DateTime.Now.ToUniversalTime();
        public UserRating? UserRating { get; set; }

        // foreign key definition
        public int WatchlistId { get; set; }
        public virtual Watchlist Watchlist { get; set; }
    }
}
