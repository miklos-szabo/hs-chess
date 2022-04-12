using HSC.Transfer.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.Hubs.Clients
{
    public interface IChessClient
    {
        Task ReceiveMove(MoveDto move);
        public Task SendMoveToServer(MoveDto move, Guid matchId);
        public Task JoinMatch(Guid matchId);
        public Task LeaveMatch(Guid matchId);

        Task ReceiveMatchFound(Guid matchId);

        Task ReceiveFold();
        Task ReceiveCheck();
        Task ReceiveCall();
        Task ReceiveBet(decimal newAmount);

        Task ReceiveFriendRequest(string fromUserName);

        Task ReceiveMessage(ChatMessageDto message);
    }
}
