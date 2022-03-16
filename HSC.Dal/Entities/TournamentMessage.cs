using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class TournamentMessage
    {
        public int Id { get; set; }
        public string SenderUserName { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Message { get; set; }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}
