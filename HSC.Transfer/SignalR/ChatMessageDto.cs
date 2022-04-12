using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.SignalR
{
    public class ChatMessageDto
    {
        public string TimeStamp { get; set; }
        public string SenderUserName { get; set; }
        public string Message { get; set; }
    }
}
