using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamKing.Data.Media
{
    public class SeasonEntry : WatchEntry
    {
        public Season Season { get; set; }
        public List<EpisodeEntry>? Episodes { get; set; } = new List<EpisodeEntry>();
    }
}
