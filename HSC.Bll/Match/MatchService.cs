using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    }
}
