using AutoMapper;
using AutoMapper.QueryableExtensions;
using HSC.Common.Extensions;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Transfer.History;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.HistoryService
{
    public class HistoryService : IHistoryService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;
        private readonly IMapper _mapper;

        public HistoryService(HSCContext dbContext, IRequestContext requestContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
            _mapper = mapper;
        }

        public async Task<List<PastGameDto>> GetPastGamesAsync(HistorySearchDto searchDto, int pageSize, int page)
        {
            // Todo search for results
            return await _dbContext.Matches
                .ProjectTo<PastGameDto>(_mapper.ConfigurationProvider)
                .Where(m => m.BlackUserName == _requestContext.UserName || m.WhiteUserName == _requestContext.UserName)
                .OrderByDescending(m => m.StartTime)
                .Where(!string.IsNullOrEmpty(searchDto.Opponent),
                    m => m.WhiteUserName == _requestContext.UserName ?
                    m.BlackUserName.ToLower().Contains(searchDto.Opponent.ToLower()) :
                    m.WhiteUserName.ToLower().Contains(searchDto.Opponent.ToLower()))
                .Where(searchDto.IntervalStart.HasValue, m => m.StartTime > searchDto.IntervalStart)
                .Where(searchDto.IntervalEnd.HasValue, m => m.StartTime > searchDto.IntervalEnd)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        }
    }
}
