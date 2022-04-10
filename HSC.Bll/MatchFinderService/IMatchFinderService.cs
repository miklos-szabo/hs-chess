using HSC.Transfer.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.MatchFinderService
{
    public interface IMatchFinderService
    {
        Task SearchForMatchAsync(SearchingForMatchDto dto);
        Task<List<CustomGameDto>> GetCustomGamesAsync();
        Task CreateCustomGameAsync(CreateCustomGameDto dto);
        Task<Guid> JoinCustomGame(int challengeId);
    }
}
