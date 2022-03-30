using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.Hubs.Clients
{
    public interface IChessClient
    {
        Task ReceiveMove(string origin, string destination, string promotion);
        public Task SendMoveToServer(string origin, string destination, string promotion);
        public Task JoinMatch(Guid matchId);
        public Task LeaveMatch(Guid matchId);
    }
}
