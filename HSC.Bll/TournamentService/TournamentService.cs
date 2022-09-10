using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HSC.Bll.Hubs.Clients;
using HSC.Bll.Hubs;
using HSC.Bll.Scheduling;
using HSC.Common.Enums;
using HSC.Common.Exceptions;
using HSC.Common.Extensions;
using HSC.Common.RequestContext;
using HSC.Dal;
using HSC.Dal.Entities;
using HSC.Transfer.Tournament;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HSC.Bll.TournamentService
{
    public class TournamentService: ITournamentService
    {
        private readonly HSCContext _dbContext;
        private readonly ILogger<TournamentService> _logger;
        private readonly IRequestContext _requestContext;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChessHub, IChessClient> _chessHub;
        private readonly HSCJobScheduler _scheduler;

        public TournamentService(HSCContext dbContext, ILogger<TournamentService> logger, IRequestContext requestContext, IMapper mapper, IHubContext<ChessHub, IChessClient> chessHub, HSCJobScheduler scheduler)
        {
            _dbContext = dbContext;
            _logger = logger;
            _requestContext = requestContext;
            _mapper = mapper;
            _chessHub = chessHub;
            _scheduler = scheduler;
        }

        public async Task CreateTournamentAsync(CreateTournamentDto dto)
        {
            var tournament = new Tournament
            {
                BuyIn = dto.BuyIn,
                Description = dto.Description,
                GameIncrement = dto.GameIncrement,
                GameTimeMinutes = dto.GameTimeMinutes,
                Length = dto.Length,
                PrizePool = dto.PrizePoolMinimum,
                StartTime = dto.StartTime,
                Title = dto.Title,
                TournamentStatus = TournamentStatus.NotStarted,
                Type = dto.Type
            };

            await _dbContext.Tournaments.AddAsync(tournament);
            await _dbContext.SaveChangesAsync();

            await _scheduler.RegisterTournamentStartJobAsync(tournament.Id, tournament.StartTime);
            await _scheduler.RegisterTournamentEndJobAsync(tournament.Id, tournament.StartTime.Add(tournament.Length));

            _logger.LogDebug("Created new tournament: {name}, id: {id}", tournament.Title, tournament.Id);
        }

        public async Task<List<TournamentListDto>> GetTournamentsAsync(SearchTournamentDto dto)
        {
            var tournaments = await _dbContext.Tournaments
                .Where(dto.PastTournaments, t => t.TournamentStatus == TournamentStatus.Finished)
                .Where(!dto.PastTournaments, t => t.TournamentStatus != TournamentStatus.Finished)
                .Where(!string.IsNullOrEmpty(dto.Title), t => t.Title.Contains(dto.Title))
                .Where(dto.StartDateIntervalStart.HasValue, t => t.StartTime >= dto.StartDateIntervalStart.Value)
                .Where(dto.StartDateIntervalEnd.HasValue, t => t.StartTime >= dto.StartDateIntervalEnd.Value)                
                .Where(dto.BuyInMin.HasValue, t => t.BuyIn >= dto.BuyInMin.Value)
                .Where(dto.BuyInMax.HasValue, t => t.BuyIn >= dto.BuyInMax.Value)
                .Take(50)
                .ProjectTo<TournamentListDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            if (dto.PastTournaments)
            {
                tournaments = tournaments.OrderByDescending(t => t.StartTime).ToList();
            }
            else
            {
                tournaments = tournaments.OrderBy(t => t.StartTime).ToList();
            }

            _logger.LogDebug("Found {count} tournaments.", tournaments.Count);

            return tournaments;
        }

        public async Task<TournamentDetailsDto> GetTournamentDetailsAsync(int id)
        {
            var details = await _dbContext.Tournaments
                .ProjectTo<TournamentDetailsDto>(_mapper.ConfigurationProvider, new {currentUserName = _requestContext.UserName})
                .SingleAsync(t => t.Id == id);

            if (details.TournamentStatus == TournamentStatus.NotStarted)
                details.StartsInEndsAt = details.StartTime.Subtract(DateTimeOffset.Now).TotalSeconds;
            else if (details.TournamentStatus == TournamentStatus.Ongoing)
                details.StartsInEndsAt = details.StartTime.Add(details.Length).Subtract(DateTimeOffset.Now).TotalSeconds;

            return details;
        }

        public async Task JoinTournamentAsync(int id)
        {
            var tournament = await _dbContext.Tournaments.Include(t => t.Players)
                .SingleAsync(t => t.Id == id);

            if (tournament.Players.Any(p => p.UserName == _requestContext.UserName))
            {
                throw new BadRequestException("Already in tournament.");
            }

            var user = await _dbContext.Users.SingleAsync(u => u.UserName == _requestContext.UserName);
            user.Balance -= tournament.BuyIn;

            if (user.Balance < 0)
            {
                throw new BadRequestException("Not enough money in the account.");
            }

            tournament.PrizePool += tournament.BuyIn;

            tournament.Players.Add(new TournamentPlayer
            {
                Points = 0,
                UserName = _requestContext.UserName,
            });

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TournamentMessageDto>> GetMessages(int id)
        {
            return await _dbContext.TournamentMessages.Where(tm => tm.TournamentId == id)
                .OrderByDescending(tm => tm.TimeStamp)
                .Take(50)
                .ProjectTo<TournamentMessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task SendMessage(int id, string message)
        {
            _dbContext.TournamentMessages.Add(new TournamentMessage
            {
                Message = message,
                SenderUserName = _requestContext.UserName,
                TimeStamp = DateTime.Now,
                TournamentId = id,
            });

            await _dbContext.SaveChangesAsync();

            var players = await _dbContext.TournamentPlayers.Where(tp => tp.TournamentId == id).Select(tp => tp.UserName).ToListAsync();
            await _chessHub.Clients.Users(players).ReceiveTournamentMessage();
        }

        public async Task SearchForNextMatch(int id)
        {
            var tournament = await _dbContext.Tournaments.SingleAsync(t => t.Id == id);
            var user = await _dbContext.TournamentPlayers.SingleAsync(tp => tp.UserName == _requestContext.UserName && tp.TournamentId == id);
            var searchingPlayers = _dbContext.TournamentPlayers.Where(tp => tp.TournamentId == id && tp.IsSearching);
            if (searchingPlayers.Any())
            {
                var differenceOrderedPlayers = searchingPlayers.OrderBy(sp => Math.Abs(sp.Points.Value - user.Points.Value));
                var otherPlayer = differenceOrderedPlayers.First();
                otherPlayer.IsSearching = false;

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
                            IsBetting = (Color)firstPlayerColor == Color.White
                        },
                        new MatchPlayer
                        {
                            UserName = _requestContext.UserName,
                            Color = (Color)(1 - firstPlayerColor),
                            IsBetting = (Color)(1 - firstPlayerColor) == Color.White
                        }
                    },
                    StartTime = DateTime.UtcNow,
                    TimeLimitMinutes = tournament.GameTimeMinutes,
                    Increment = tournament.GameIncrement,
                    MinimumBet = 0,
                    MaximumBet = 0,
                    TournamentId = tournament.Id
                };

                _dbContext.Matches.Add(match);
                await _dbContext.SaveChangesAsync();
                await _chessHub.Clients.Users(new List<string> { otherPlayer.UserName, _requestContext.UserName })
                    .ReceiveMatchFound(match.Id);
            }
            else
            {
                user.IsSearching = true;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<TournamentPlayerDto>> GetStandingsAsync(int tournamentId)
        {
            return await _dbContext.TournamentPlayers.Where(tp => tp.TournamentId == tournamentId)
                .ProjectTo<TournamentPlayerDto>(_mapper.ConfigurationProvider)
                .OrderByDescending(tp => tp.Points).ToListAsync();
        }
    }
}
