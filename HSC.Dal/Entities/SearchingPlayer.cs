using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class SearchingPlayer
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public TimeSpan Increment { get; set; }
        public double MinimumBet { get; set; }
        public double MaximumBet { get; set; }
        public int Rating { get; set; }
    }
}
