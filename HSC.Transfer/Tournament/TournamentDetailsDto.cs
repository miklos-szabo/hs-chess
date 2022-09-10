using HSC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Tournament
{
    public class TournamentDetailsDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public double StartsInEndsAt { get; set; }
        public TimeSpan Length { get; set; }
        public int GameTimeMinutes { get; set; }
        public int GameIncrement { get; set; }
        public TournamentType Type { get; set; }
        public string WinnerUserName { get; set; }
        public decimal BuyIn { get; set; }
        public decimal PrizePool { get; set; }
        public TournamentStatus TournamentStatus { get; set; }
        public bool HasJoined { get; set; }
    }
}
