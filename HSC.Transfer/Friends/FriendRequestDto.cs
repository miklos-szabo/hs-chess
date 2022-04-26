using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Friends
{
    public class FriendRequestDto
    {
        public int Id { get; set; }
        public string RequesterUserName { get; set; }
        public string ReceiverUserName { get; set; }
        public bool IsIncoming { get; set; }
    }
}
