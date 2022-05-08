using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Common.Enums;

namespace HSC.Transfer.Match
{
    public class MatchFullDataDto
    {
        public string BlackUserName { get; set; }
        public string BlackRating { get; set; }
        public string WhiteUserName { get; set; }
        public string WhiteRating { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int Increment { get; set; }
        public decimal MinimumBet { get; set; }
        public decimal MaximumBet { get; set; }
        public decimal FinalPot { get; set; }
        public bool IsHistoryMode { get; set; }
        public string Moves { get; set; }
    }
}
