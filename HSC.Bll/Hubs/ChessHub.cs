using HSC.Bll.Hubs.Clients;
using HSC.Transfer.SignalR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.Hubs
{
    public class ChessHub: Hub<IChessClient>
    {
        public Task JoinMatch(Guid matchId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, matchId.ToString());
        }

        public Task LeaveMatch(Guid matchId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId.ToString());
        }

        public Task SendMoveToServer(MoveDto move, Guid matchId)
        {
            return Clients.OthersInGroup(matchId.ToString()).ReceiveMove(move);
        }
    }
}
