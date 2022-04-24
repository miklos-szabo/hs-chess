using HSC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class MatchPlayer
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Color Color { get; set; }
        public int Rating { get; set; }
        public bool IsWinner { get; set; }
        public decimal CurrentBet { get; set; }
        public bool IsBetting { get; set; }

        public Guid MatchId { get; set; }
        public Match Match { get; set; }
    }
}
