using HSC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class Tournament
    {
        public Tournament()
        {
            Matches = new HashSet<Match>();
            Players = new HashSet<TournamentPlayer>();
            Messages = new HashSet<TournamentMessage>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public TimeSpan Length { get; set; }
        public TimeSpan GameTime { get; set; }
        public TimeSpan GameIncrement { get; set; }
        public TournamentType Type { get; set; }
        public string WinnerUserName { get; set; }
        public decimal BuyIn { get; set; }
        public decimal PrizePool { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
        public virtual ICollection<TournamentPlayer> Players { get; set; }
        public virtual ICollection<TournamentMessage> Messages { get; set; }
    }
}
