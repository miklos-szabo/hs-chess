using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Match
{
    public class MatchStartDto
    {
        public string BlackUserName { get; set; }
        public string BlackRating { get; set; }
        public string WhiteUserName { get; set; }
        public string WhiteRating { get; set; }
    }
}
