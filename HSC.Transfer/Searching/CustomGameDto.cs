using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Searching
{
    public class CustomGameDto
    {
        public int Id { get; set; }
        public string Offerer { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int Increment { get; set; }
        public decimal MinimumBet { get; set; }
        public decimal MaximumBet { get; set; }
        public string Receiver { get; set; }
        public bool IsToMe { get; set; }
    }
}
