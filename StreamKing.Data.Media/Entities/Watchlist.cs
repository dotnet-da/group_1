using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamKing.Data.Media
{
    public class Watchlist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now.ToUniversalTime();
        //public List<WatchEntry> List { get; set; } = new List<WatchEntry>();
        public List<MovieEntry> MovieList { get; set; } = new List<MovieEntry>();
        public List<SeriesEntry> SeriesList { get; set; } = new List<SeriesEntry>();
    }
}
