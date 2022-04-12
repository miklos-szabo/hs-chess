using HSC.Transfer.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.Match
{
    public interface IMatchService
    {
        Task<MatchStartDto> GetMatchStartingDataAsync(Guid matchId);
    }
}
