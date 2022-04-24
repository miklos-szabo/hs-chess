using HSC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.History
{
    public class PastGameDto
    {
        public Guid Id { get; set; }
        public string BlackUserName { get; set; }
        public string BlackRating { get; set; }
        public string WhiteUserName { get; set; }
        public string WhiteRating { get; set; }
        public Result Result { get; set; }
        public decimal BetAmount { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int Increment { get; set; }
        public DateTimeOffset StartTime { get; set; }
    }
}
