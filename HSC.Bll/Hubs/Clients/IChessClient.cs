using HSC.Transfer.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Common.Enums;

namespace HSC.Bll.Hubs.Clients
{
    public interface IChessClient
    {
        Task ReceiveMove(MoveDto move);
        public Task SendMoveToServer(MoveDto move, Guid matchId);
        public Task JoinMatch(Guid matchId);
        public Task LeaveMatch(Guid matchId);
        public Task SendDrawOfferToUser(string toUserName);

        Task ReceiveMatchFound(Guid matchId);
        Task ReceiveChallenge(ChallengeDto challenge);

        Task ReceiveFold(decimal finalAmount);
        Task ReceiveCheck();
        Task ReceiveCall();
        Task ReceiveBet(decimal newAmount);

        Task ReceiveDrawOffer();
        Task ReceiveGameEnded(Result result);

        Task ReceiveFriendRequest(string fromUserName);

        Task ReceiveMessage(ChatMessageDto message);
    }
}
