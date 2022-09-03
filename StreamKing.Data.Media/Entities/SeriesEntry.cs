using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamKing.Data.Media
{
    public class SeriesEntry : WatchEntry
    {
        public Series Series { get; set; }
        public List<SeasonEntry>? Seasons { get; set; }
    }
}
