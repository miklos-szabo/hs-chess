using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Tournament
{
    public class TournamentMessageDto
    {
        public string SenderUserName { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Message { get; set; }
    }
}
