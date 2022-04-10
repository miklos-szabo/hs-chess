using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class Challenge
    {
        public int Id { get; set; }
        public string Offerer { get; set; }
        public string Receiver { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public TimeSpan Increment { get; set; }
        public decimal MinimumBet { get; set; }
        public decimal MaximumBet { get; set; }
    }
}
