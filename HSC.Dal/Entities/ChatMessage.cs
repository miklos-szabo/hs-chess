using System;
using System.Collections.Generic;
using System.Text;

namespace HSC.Dal.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string Message { get; set; }
        public bool IsSeen { get; set; }
    }
}
