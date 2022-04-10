using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.BettingService
{
    public class BettingService : IBettingService
    {
        public Task CallAsnyc(Guid matchId, string userName)
        {
            throw new NotImplementedException();
        }

        public Task CheckAsync(Guid matchId, string userName)
        {
            throw new NotImplementedException();
        }

        public Task FoldAsync(Guid matchId, string userName)
        {
            throw new NotImplementedException();
        }

        public Task RaiseAsync(Guid matchId, string userName, decimal newAmount)
        {
            throw new NotImplementedException();
        }
    }
}
