using HSC.Bll.Hubs.Clients;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task SendMoveToServer(string origin, string destination, string promotion)
        {
            return Clients.Others.ReceiveMove(origin, destination, promotion);
        }
    }
}
