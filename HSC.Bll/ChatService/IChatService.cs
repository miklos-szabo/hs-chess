using HSC.Transfer.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.ChatService
{
    public interface IChatService
    {
        Task SendChatMessage(string toUserName, string message);
        Task<List<ChatMessageDto>> GetChatMessages(string fromUserName, int pageSize, int page);
        Task MessagesRead(string fromUserName);
    }
}
