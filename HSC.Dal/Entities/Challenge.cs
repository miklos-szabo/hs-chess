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
        public int TimeLimitMinutes { get; set; }
        public int Increment { get; set; }
        public decimal MinimumBet { get; set; }
        public decimal MaximumBet { get; set; }
    }
}
