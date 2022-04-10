using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.BettingService
{
    public interface IBettingService
    {
        Task CheckAsync(Guid matchId, string userName);
        Task CallAsnyc(Guid matchId, string userName);
        Task RaiseAsync(Guid matchId, string userName, decimal newAmount);
        Task FoldAsync(Guid matchId, string userName);
    }
}
