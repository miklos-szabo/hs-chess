﻿using HSC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Tournament
{
    public class CreateTournamentDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Length { get; set; }
        public int GameTimeMinutes { get; set; }
        public int GameIncrement { get; set; }
        public TournamentType Type { get; set; }
        public decimal BuyIn { get; set; }
        public decimal PrizePoolMinimum { get; set; }
    }
}
