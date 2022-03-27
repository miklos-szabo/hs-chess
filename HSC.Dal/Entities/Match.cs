﻿using HSC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class Match
    {
        public Match()
        {
            MatchPlayers = new HashSet<MatchPlayer>();
        }

        public Guid Id { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public TimeSpan Increment { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public Result Result { get; set; }
        public string Moves { get; set; }
        public double MinimumBet { get; set; }
        public double MaximumBet { get; set; }
        public double CurrentBet { get; set; }

        public int? TournamentId { get; set; }
        public Tournament Tournament { get; set; }

        public virtual ICollection<MatchPlayer> MatchPlayers { get; set; }
    }
}
