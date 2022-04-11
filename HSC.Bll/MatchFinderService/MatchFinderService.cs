using HSC.Common.RequestContext;
using HSC.Dal;
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
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;

        public MatchFinderService(HSCContext context, IRequestContext requestContext)
        {
            _dbContext = context;
            _requestContext = requestContext;
        }

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

        public async Task SearchForMatchAsync(SearchingForMatchDto dto)
        {
            var un = _requestContext.UserName;
        }
    }
}
