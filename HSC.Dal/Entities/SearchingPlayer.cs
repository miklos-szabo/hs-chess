﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class SearchingPlayer
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public TimeSpan Increment { get; set; }
        public decimal MinimumBet { get; set; }
        public decimal MaximumBet { get; set; }
        public int Rating { get; set; }
    }
}
