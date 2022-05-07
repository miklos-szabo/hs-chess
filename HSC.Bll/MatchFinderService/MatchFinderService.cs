using AutoMapper;
using AutoMapper.QueryableExtensions;
using HSC.Bll.Hubs;
using HSC.Bll.Hubs.Clients;
using HSC.Bll.RatingService;
using HSC.Common.Enums;
using HSC.Common.Extensions;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Dal.Entities;
using HSC.Transfer.Searching;
using HSC.Transfer.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HSC.Bll.MatchFinderService
{
    public class MatchFinderService : IMatchFinderService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;
        private readonly IHubContext<ChessHub, IChessClient> _chessHub;
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;

        public MatchFinderService(HSCContext context, IRequestContext requestContext, IHubContext<ChessHub, IChessClient> chessHub, IMapper mapper, IRatingService ratingService)
        {
            _dbContext = context;
            _requestContext = requestContext;
            _chessHub = chessHub;
            _mapper = mapper;
            _ratingService = ratingService;
        }

        public async Task CreateCustomGameAsync(CreateCustomGameDto dto)
        {
            var challenge = new Challenge
            {
                Increment = dto.Increment,
                TimeLimitMinutes = dto.TimeLimitMinutes,
                MinimumBet = dto.MinimumBet,
                MaximumBet = dto.MaximumBet,
                Offerer = _requestContext.UserName,
                Receiver = dto.UserName,
            };

            _dbContext.Challenges.Add(challenge);
            await _dbContext.SaveChangesAsync();

            if(!string.IsNullOrEmpty(challenge.Receiver))
            {
                await _chessHub.Clients.User(challenge.Receiver).ReceiveChallenge(new ChallengeDto{Id = challenge.Id, UserName = challenge.Offerer});
            }
        }

        public async Task<List<CustomGameDto>> GetCustomGamesAsync()
        {
            var challenges = await _dbContext.Challenges
                .Where(c => string.IsNullOrEmpty(c.Receiver) || c.Receiver == _requestContext.UserName)
                .ProjectTo<CustomGameDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            challenges.ForEach(c => c.IsToMe = c.Receiver == _requestContext.UserName);

            return challenges.OrderBy(c => c.IsToMe).ToList();
        }

        public async Task JoinCustomGame(int challengeId)
        {
            var challenge = await _dbContext.Challenges.SingleAsync(c => c.Id == challengeId);
            _dbContext.Challenges.Remove(challenge);

            var me = await _dbContext.Users.SingleAsync(u => u.UserName == _requestContext.UserName);
            var myRating = _ratingService.GetRatingOfUserFromTimeControl(me, challenge.TimeLimitMinutes);

            var otherUser = await _dbContext.Users.SingleAsync(u => u.UserName == challenge.Offerer);
            var otherUserRating = _ratingService.GetRatingOfUserFromTimeControl(otherUser, challenge.TimeLimitMinutes);

            Random rd = new Random();
            var firstPlayerColor = rd.Next(0, 1);

            var match = new Dal.Entities.Match
            {
                MatchPlayers = new List<MatchPlayer>
                {
                    new MatchPlayer
                    {
                        UserName = challenge.Offerer,
                        Color = (Color)firstPlayerColor,
                        Rating = otherUserRating,
                        CurrentBet = 0,
                        IsBetting = (Color)firstPlayerColor == Color.White
                    },
                    new MatchPlayer
                    {
                        UserName = _requestContext.UserName,
                        Color = (Color)(1 - firstPlayerColor),
                        Rating = myRating,
                        CurrentBet = 0,
                        IsBetting = (Color)(1 - firstPlayerColor) == Color.White
                    }
                },
                StartTime = DateTime.UtcNow,
                TimeLimitMinutes = challenge.TimeLimitMinutes,
                Increment = challenge.Increment,
                MinimumBet = challenge.MinimumBet,
                MaximumBet = challenge.MaximumBet,
            };

            _dbContext.Matches.Add(match);
            await _dbContext.SaveChangesAsync();

            await _chessHub.Clients.Users(new List<string> { otherUser.UserName, _requestContext.UserName })
                .ReceiveMatchFound(match.Id);
        }

        public async Task SearchForMatchAsync(SearchingForMatchDto dto)
        {
            var me = await _dbContext.Users.SingleAsync(u => u.UserName == _requestContext.UserName);
            var myRating = _ratingService.GetRatingOfUserFromTimeControl(me, dto.TimeLimitMinutes);

            var otherPlayer = await _dbContext.SearchingPlayers
                .Where(sp => sp.Rating <= myRating + 50 && sp.Rating >= myRating - 50)
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
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _dbContext.SearchingPlayers.Remove(otherPlayer);
                var otherUser = await _dbContext.Users.SingleAsync(u => u.UserName == otherPlayer.UserName);
                var otherUserRating = _ratingService.GetRatingOfUserFromTimeControl(otherUser, dto.TimeLimitMinutes);

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
                            Rating = otherUserRating,
                            CurrentBet = 0,
                            IsBetting = (Color)firstPlayerColor == Color.White
                        },
                        new MatchPlayer
                        {
                            UserName = _requestContext.UserName,
                            Color = (Color)(1 - firstPlayerColor),
                            Rating = myRating,
                            CurrentBet = 0,
                            IsBetting = (Color)(1 - firstPlayerColor) == Color.White
                        }
                    },
                    StartTime = DateTime.UtcNow,
                    TimeLimitMinutes = dto.TimeLimitMinutes,
                    Increment = dto.Increment,
                    MinimumBet = dto.MinimumBet,
                    MaximumBet = dto.MaximumBet,
                };

                _dbContext.Matches.Add(match);
                await _dbContext.SaveChangesAsync();

                await _chessHub.Clients.Users(new List<string> { otherPlayer.UserName, _requestContext.UserName })
                    .ReceiveMatchFound(match.Id);
            }
        }
    }
}
