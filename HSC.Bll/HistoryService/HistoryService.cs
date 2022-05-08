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
using HSC.Common.Constants;
using HSC.Common.Enums;

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
            var matches = await _dbContext.Matches
                .Where(m => m.Result != Result.Ongoing)
                .Where(searchDto.SearchSimpleResult == SearchSimpleResult.Victory, m => m.MatchPlayers.Single(mp => mp.UserName == _requestContext.UserName).IsWinner)
                .Where(searchDto.SearchSimpleResult == SearchSimpleResult.Defeat, m => m.MatchPlayers.Single(mp => mp.UserName != _requestContext.UserName).IsWinner)
                .Where(searchDto.SearchSimpleResult == SearchSimpleResult.Draw, m => ResultTypes.Draw.Contains(m.Result))
                .ProjectTo<PastGameDto>(_mapper.ConfigurationProvider)
                .Where(m => m.BlackUserName == _requestContext.UserName || m.WhiteUserName == _requestContext.UserName)
                .OrderByDescending(m => m.StartTime)
                .Where(!string.IsNullOrEmpty(searchDto.Opponent),
                    m => m.WhiteUserName == _requestContext.UserName ?
                    m.BlackUserName.ToLower().Contains(searchDto.Opponent.ToLower()) :
                    m.WhiteUserName.ToLower().Contains(searchDto.Opponent.ToLower()))
                .Where(searchDto.IntervalStart.HasValue, m => m.StartTime > searchDto.IntervalStart)
                .Where(searchDto.IntervalEnd.HasValue, m => m.StartTime < searchDto.IntervalEnd)
                .Skip(page * pageSize).Take(pageSize)
                .ToListAsync();

            matches.ForEach(m =>
            {
                if (ResultTypes.Draw.Contains(m.Result))
                {
                    m.SearchSimpleResult = SearchSimpleResult.Draw;
                    return;
                }

                var currentUserColor = m.BlackUserName == _requestContext.UserName ? Color.Black : Color.White;

                if (currentUserColor == Color.Black && ResultTypes.BlackWon.Contains(m.Result)
                    || currentUserColor == Color.White && ResultTypes.WhiteWon.Contains(m.Result))
                {
                    m.SearchSimpleResult = SearchSimpleResult.Victory;
                    return;
                }

                m.SearchSimpleResult = SearchSimpleResult.Defeat;
            });

            return matches;
        }
    }
}
