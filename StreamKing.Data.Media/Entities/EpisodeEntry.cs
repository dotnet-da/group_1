using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamKing.Data.Media
{
    public class EpisodeEntry : WatchEntry
    {
        public Episode Episode { get; set; }

        // Foreign key for navigation
        public SeasonEntry SeasonEntry { get; set; }
    }
}
