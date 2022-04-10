using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class TournamentPlayer
    {
        public int TournamentId { get; set; }
        public string UserName { get; set; }
        public decimal? Points { get; set; }

        public Tournament Tournament { get; set; }
    }
}
