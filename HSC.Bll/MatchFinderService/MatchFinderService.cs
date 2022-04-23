using HSC.Bll.Hubs;
using HSC.Bll.Hubs.Clients;
using HSC.Common.Enums;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Dal.Entities;
using HSC.Transfer.Searching;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HSC.Bll.MatchFinderService
{
    public class MatchFinderService : IMatchFinderService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;
        private readonly IHubContext<ChessHub, IChessClient> _chessHub;

        public MatchFinderService(HSCContext context, IRequestContext requestContext, IHubContext<ChessHub, IChessClient> chessHub)
        {
            _dbContext = context;
            _requestContext = requestContext;
            _chessHub = chessHub;
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
            var otherPlayer = await _dbContext.SearchingPlayers
                .Where(sp => sp.Rating == 1000) // TODO rating
                .Where(sp => sp.TimeLimitMinutes == dto.TimeLimitMinutes)
                .Where(sp => sp.Increment == dto.Increment)
                .Where(sp => sp.MinimumBet == dto.MinimumBet)
                .Where(sp => sp.MaximumBet == dto.MaximumBet)
                .FirstOrDefaultAsync();

            if (otherPlayer == null)
            {
                _dbContext.SearchingPlayers.Add(new Dal.Entities.SearchingPlayer
                {
                    UserName = dto.UserName,
                    Rating = 1000,
                    TimeLimitMinutes = dto.TimeLimitMinutes,
                    Increment = dto.Increment,
                    MinimumBet = dto.MinimumBet,
                    MaximumBet = dto.MaximumBet,
                });
            }
            else
            {
                _dbContext.SearchingPlayers.Remove(otherPlayer);

                Random rd = new Random();
                var firstPlayerColor = rd.Next(0, 1);

                var match = new Dal.Entities.Match
                {
                    MatchPlayers = new List<MatchPlayer>()
                    {
                        new MatchPlayer
                        {
                            UserName = otherPlayer.UserName,
                            Color = (Color)firstPlayerColor,
                            Rating = 1000,
                        },
                        new MatchPlayer
                        {
                            UserName = _requestContext.UserName,
                            Color = (Color)(1 - firstPlayerColor),
                            Rating = 1000,
                        }
                    },
                    StartTime = DateTime.UtcNow,
                    TimeLimitMinutes = dto.TimeLimitMinutes,
                    Increment = dto.Increment,
                    MinimumBet = dto.MinimumBet,
                    MaximumBet = dto.MaximumBet,
                    CurrentBet = dto.MinimumBet,
                };

                _dbContext.Matches.Add(match);

                await _chessHub.Clients.Users(new List<string> { otherPlayer.UserName, _requestContext.UserName })
                    .ReceiveMatchFound(match.Id);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
