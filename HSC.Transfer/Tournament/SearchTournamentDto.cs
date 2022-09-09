using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Tournament
{
    public class SearchTournamentDto
    {
        public bool PastTournaments { get; set; }
        public string Title { get; set; }
        public DateTimeOffset? StartDateIntervalStart { get; set; }
        public DateTimeOffset? StartDateIntervalEnd { get; set; }
        public decimal? BuyInMin { get; set; }
        public decimal? BuyInMax { get; set; }
    }
}
