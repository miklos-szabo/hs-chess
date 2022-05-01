using HSC.Common.Enums;
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
        Task<MatchFullDataDto> GetMatchDataAsync(Guid matchId);
        Task MatchOver(Guid matchId, Result result, string winnerUserName);
        Task SaveMatchPgn(Guid matchId, string pgn);
        Task<string> GetMatchPgn(Guid matchId);
    }
}
