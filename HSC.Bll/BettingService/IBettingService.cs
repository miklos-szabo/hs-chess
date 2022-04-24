using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.BettingService
{
    public interface IBettingService
    {
        Task CheckAsync(Guid matchId);
        Task CallAsnyc(Guid matchId);
        Task RaiseAsync(Guid matchId, decimal newAmount);
        Task FoldAsync(Guid matchId);
    }
}
