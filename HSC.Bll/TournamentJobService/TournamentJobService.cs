using HSC.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Dal;
using Microsoft.Extensions.Logging;
using HSC.Dal.Entities;
using HSC.Bll.Hubs.Clients;
using HSC.Bll.Hubs;
using HSC.Transfer.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace HSC.Bll.TournamentJobService
{
    public class TournamentJobService: ITournamentJobService
    {
        private readonly HSCContext _dbContext;
        private readonly ILogger<TournamentJobService> _logger;
        private readonly IHubContext<ChessHub, IChessClient> _chessHub;

        public TournamentJobService(HSCContext dbContext, ILogger<TournamentJobService> logger, IHubContext<ChessHub, IChessClient> chessHub)
        {
            _dbContext = dbContext;
            _logger = logger;
            _chessHub = chessHub;
        }

        public async Task TournamentOver(int id)
        {
            var tournament = await _dbContext.Tournaments.Include(t => t.Players).Include(t => t.Matches)
                .SingleAsync(t => t.Id == id);

            var ongoingMatches = tournament.Matches.Where(m => m.Result == Result.Ongoing).ToList();

            foreach (var match in ongoingMatches)
            {
                match.Result = Result.DrawByAgreement;
            }

            tournament.WinnerUserName = tournament.Players.Any() ? tournament.Players.OrderByDescending(p => p.Points).First().UserName : "-";
            tournament.TournamentStatus = TournamentStatus.Finished;
            await _dbContext.SaveChangesAsync();

            await CalculateAndSendWinnings(tournament);

            _logger.LogDebug("Tournament {name}, id: {id} is over.", tournament.Title, tournament.Id);
        }

        public async Task TournamentStart(int id)
        {
            var tournament = await _dbContext.Tournaments.Include(t => t.Players).SingleAsync(t => t.Id == id);

            tournament.TournamentStatus = TournamentStatus.Ongoing;
            await _dbContext.SaveChangesAsync();

            var players = tournament.Players.Select(p => p.UserName).ToList();
            await _chessHub.Clients.Users(players).ReceiveTournamentStarted();

            _logger.LogDebug("Tournament {name}, id: {id} has started.", tournament.Title, id);
        }

        public async Task CalculateAndSendWinnings(Tournament t)
        {
            var winnings = new Dictionary<string, decimal>();
            var players = t.Players.OrderByDescending(p => p.Points).ToList();

            if (players.Count == 0) return;
            if (players.Count == 1 || players.Count == 2 || players.Count == 3)
            {
                winnings.Add(players.First().UserName, t.PrizePool);
            }
            else if(players.Count > 3 && players.Count < 8)
            {
                winnings.Add(players.First().UserName, t.PrizePool * 0.65m);
                winnings.Add(players.Skip(1).First().UserName, t.PrizePool * 0.35m);
            }
            else if(players.Count >= 8 && players.Count < 12)
            {
                winnings.Add(players.First().UserName, t.PrizePool * 0.5m);
                winnings.Add(players.Skip(1).First().UserName, t.PrizePool * 0.3m);
                winnings.Add(players.Skip(2).First().UserName, t.PrizePool * 0.2m);
            }
            else
            {
                var cutOver = (int)Math.Round(players.Count * 0.3);

                winnings.Add(players.First().UserName, t.PrizePool * 0.4m);
                winnings.Add(players.Skip(1).First().UserName, t.PrizePool * 0.25m);
                winnings.Add(players.Skip(2).First().UserName, t.PrizePool * 0.20m);

                var lowPrize = t.PrizePool / (cutOver - 3);

                for (int i = 3; i < cutOver; i++)
                {
                    winnings.Add(players.Skip(i).First().UserName, t.PrizePool * lowPrize);
                }
            }

            var users = await _dbContext.Users.Where(u => players.Select(p => p.UserName).Contains(u.UserName)).ToListAsync();
            foreach (var winning in winnings)
            {
                users.Single(u => u.UserName == winning.Key).Balance += winning.Value;
            }

            await _dbContext.SaveChangesAsync();

            foreach (var player in players)
            {
                if(!winnings.Keys.Contains(player.UserName))
                    winnings.Add(player.UserName, 0);
            }

            foreach (var winning in winnings)
            {
                await _chessHub.Clients.User(winning.Key).ReceiveTournamentOver(new TournamentOverDto
                {
                    Winner = t.WinnerUserName,
                    Winnings = winning.Value,
                });
            }

        }
    }
}
