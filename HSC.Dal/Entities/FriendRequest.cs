using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public string RequesterUsername { get; set; }
        public string ReceiverUsername { get; set; }
    }
}
