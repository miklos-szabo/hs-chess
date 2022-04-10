using HSC.Transfer.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.MatchFinderService
{
    public class MatchFinderService : IMatchFinderService
    {
        public Task CreateCustomGameAsync(CreateCustomGameDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<List<CustomGameDto>> GetCustomGamesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Guid> JoinCustomGame(int challengeId)
        {
            throw new NotImplementedException();
        }

        public Task SearchForMatchAsync(SearchingForMatchDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
