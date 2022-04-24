using AutoMapper;
using AutoMapper.QueryableExtensions;
using HSC.Common.Enums;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Transfer.Match;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.Match
{
    public class MatchService : IMatchService
    {
        private readonly HSCContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRequestContext _requestContext;

        public MatchService(HSCContext dbContext, IMapper mapper, IRequestContext requestContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _requestContext = requestContext;
        }

        public async Task<MatchStartDto> GetMatchStartingDataAsync(Guid matchId)
        {
            return await _dbContext.Matches
                .Where(m => m.Id == matchId)
                .ProjectTo<MatchStartDto>(_mapper.ConfigurationProvider)
                .FirstAsync();
        }

        public async Task MatchOver(Guid matchId, Result result, string winnerUserName)
        {
            var match = await _dbContext.Matches.Include(m => m.MatchPlayers).SingleAsync(m => m.Id == matchId);
            match.Result = result;
            match.MatchPlayers.Single(mp => mp.UserName == winnerUserName).IsWinner = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveMatchPgn(Guid matchId, string pgn)
        {
            var match = await _dbContext.Matches.SingleAsync(m => m.Id == matchId);

            match.Moves = pgn;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetMatchPgn(Guid matchId)
        {
            return (await _dbContext.Matches.SingleAsync(m => m.Id == matchId)).Moves;
        }
    }
}
