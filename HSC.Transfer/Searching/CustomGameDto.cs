﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Searching
{
    public class CustomGameDto
    {
        public int ChallengeId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public TimeSpan Increment { get; set; }
        public decimal MinimumBet { get; set; }
        public decimal MaximumBet { get; set; }
    }
}
