using AutoMapper;
using AutoMapper.QueryableExtensions;
using HSC.Bll.Hubs.Clients;
using HSC.Bll.Hubs;
using HSC.Common.Enums;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Transfer.Match;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Bll.RatingService;
using HSC.Common.Constants;
using HSC.Dal.Entities;

namespace HSC.Bll.Match
{
    public class MatchService : IMatchService
    {
        private readonly HSCContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRequestContext _requestContext;
        private readonly IHubContext<ChessHub, IChessClient> _chessHub;
        private readonly IRatingService _ratingService;

        public MatchService(HSCContext dbContext, IMapper mapper, IRequestContext requestContext, IHubContext<ChessHub, IChessClient> chessHub, IRatingService ratingService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _requestContext = requestContext;
            _chessHub = chessHub;
            _ratingService = ratingService;
        }

        public async Task<MatchStartDto> GetMatchStartingDataAsync(Guid matchId)
        {
            return await _dbContext.Matches
                .Where(m => m.Id == matchId)
                .ProjectTo<MatchStartDto>(_mapper.ConfigurationProvider)
                .FirstAsync();
        }

        public async Task<MatchFullDataDto> GetMatchDataAsync(Guid matchId)
        {
            return await _dbContext.Matches
                .Where(m => m.Id == matchId)
                .ProjectTo<MatchFullDataDto>(_mapper.ConfigurationProvider)
                .FirstAsync();
        }

        public async Task MatchOver(Guid matchId, Result result, string winnerUserName)
        {
            var match = await _dbContext.Matches.Include(m => m.MatchPlayers).SingleAsync(m => m.Id == matchId);
            var otherUserName = match.MatchPlayers.Single(mp => mp.UserName != _requestContext.UserName).UserName;
            match.Result = result;

            var thisUser = await _dbContext.Users.SingleAsync(u => u.UserName == _requestContext.UserName);
            var otherUser = await _dbContext.Users.SingleAsync(u => u.UserName == otherUserName);

            if (!ResultTypes.Draw.Contains(result))
            {
                match.MatchPlayers.Single(mp => mp.UserName == winnerUserName).IsWinner = true;
                if (winnerUserName == thisUser.UserName)
                {
                    _ratingService.ModifyRating(thisUser, match.TimeLimitMinutes, 8);
                    _ratingService.ModifyRating(otherUser, match.TimeLimitMinutes, -8);

                    thisUser.Balance += match.MatchPlayers.Max(mp => mp.CurrentBet);
                    otherUser.Balance -= match.MatchPlayers.Max(mp => mp.CurrentBet);
                }
                else
                {
                    _ratingService.ModifyRating(otherUser, match.TimeLimitMinutes, 8);
                    _ratingService.ModifyRating(thisUser, match.TimeLimitMinutes, -8);

                    otherUser.Balance += match.MatchPlayers.Max(mp => mp.CurrentBet);
                    thisUser.Balance -= match.MatchPlayers.Max(mp => mp.CurrentBet);
                }
            }

            if (match.TournamentId.HasValue)
            {
                var thisTPlayer = await _dbContext.TournamentPlayers.SingleAsync(tp =>
                    tp.TournamentId == match.TournamentId.Value && tp.UserName == _requestContext.UserName);
                var otherTPlayer = await _dbContext.TournamentPlayers.SingleAsync(tp =>
                    tp.TournamentId == match.TournamentId.Value && tp.UserName == otherUserName);

                if (ResultTypes.Draw.Contains(result))
                {
                    thisTPlayer.Points += 0.5m;
                    otherTPlayer.Points += 0.5m;
                }
                else
                {
                    if (winnerUserName == thisTPlayer.UserName)
                    {
                        thisTPlayer.Points += 1;
                    }
                    else
                    {
                        otherTPlayer.Points += 1;
                    }
                }
            }

            await _dbContext.SaveChangesAsync();

            // With these results, the game ended client-side, so we send the result to the other user.
            if (result == Result.BlackWonByTimeOut || result == Result.WhiteWonByTimeout ||
                result == Result.BlackWonByResignation || result == Result.WhiteWonByResignation ||
                result == Result.DrawByAgreement)
            {
                await _chessHub.Clients.User(otherUser.UserName).ReceiveGameEnded(result);
            }

            if (match.TournamentId.HasValue)
            {
                var tournamentPlayers = await _dbContext.TournamentPlayers.Where(tp => tp.TournamentId == match.TournamentId)
                    .Select(tp => tp.UserName).ToListAsync();
                await _chessHub.Clients.Users(tournamentPlayers).ReceiveUpdateStandings();
            }
        }

        public async Task SaveMatchPgn(Guid matchId, string pgn)
        {
            var match = await _dbContext.Matches.SingleAsync(m => m.Id == matchId);

            if (!string.IsNullOrEmpty(match.Moves)) return;
            match.Moves = pgn;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GetMatchPgn(Guid matchId)
        {
            return (await _dbContext.Matches.SingleAsync(m => m.Id == matchId)).Moves;
        }
    }
}
