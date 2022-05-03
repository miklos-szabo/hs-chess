using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.SignalR
{
    public class MoveDto
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Promotion { get; set; }
        public int? TimeLeft { get; set; }
    }
}
