using HSC.Bll.Hubs.Clients;
using HSC.Bll.Hubs;
using HSC.Common.RequestContext;
using HSC.Dal;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HSC.Common.Exceptions;

namespace HSC.Bll.BettingService
{
    public class BettingService : IBettingService
    {
        private readonly HSCContext _dbContext;
        private readonly IRequestContext _requestContext;
        private readonly IHubContext<ChessHub, IChessClient> _chessHub;

        public BettingService(HSCContext dbContext, IRequestContext requestContext, IHubContext<ChessHub, IChessClient> chessHub)
        {
            _dbContext = dbContext;
            _requestContext = requestContext;
            _chessHub = chessHub;
        }

        public async Task CallAsnyc(Guid matchId)
        {
            var match = await _dbContext.Matches.Include(m => m.MatchPlayers)
                .SingleAsync(m => m.Id == matchId);

            var currentPlayer = match.MatchPlayers.SingleOrDefault(p => p.IsBetting);

            if (currentPlayer.UserName != _requestContext.UserName)
                throw new BadRequestException("Wrong user tried to bet, it's not their turn.");

            var otherPlayer = match.MatchPlayers.SingleOrDefault(p => !p.IsBetting);

            currentPlayer.CurrentBet = otherPlayer.CurrentBet;
            currentPlayer.IsBetting = false;

            await _dbContext.SaveChangesAsync();

            await _chessHub.Clients.User(otherPlayer.UserName).ReceiveCall();
        }

        public async Task CheckAsync(Guid matchId)
        {
            var match = await _dbContext.Matches.Include(m => m.MatchPlayers)
                .SingleAsync(m => m.Id == matchId);

            var currentPlayer = match.MatchPlayers.SingleOrDefault(p => p.IsBetting);

            if (currentPlayer.UserName != _requestContext.UserName)
                throw new BadRequestException("Wrong user tried to bet, it's not their turn.");

            var otherPlayer = match.MatchPlayers.SingleOrDefault(p => !p.IsBetting);

            currentPlayer.CurrentBet = match.MinimumBet;
            currentPlayer.IsBetting = false;
            otherPlayer.IsBetting = true;

            await _dbContext.SaveChangesAsync();

            await _chessHub.Clients.User(otherPlayer?.UserName).ReceiveCheck();
        }

        public async Task FoldAsync(Guid matchId)
        {
            var match = await _dbContext.Matches.Include(m => m.MatchPlayers)
                .SingleAsync(m => m.Id == matchId);

            var currentPlayer = match.MatchPlayers.SingleOrDefault(p => p.IsBetting);

            if (currentPlayer.UserName != _requestContext.UserName)
                throw new BadRequestException("Wrong user tried to bet, it's not their turn.");

            var otherPlayer = match.MatchPlayers.SingleOrDefault(p => !p.IsBetting);

            var finalBet = Math.Clamp(Math.Max(match.MinimumBet * 1.25m, currentPlayer.CurrentBet * 1.25m), match.MinimumBet, match.MaximumBet);
            currentPlayer.CurrentBet = finalBet;
            otherPlayer.CurrentBet = finalBet;
            currentPlayer.IsBetting = false;

            await _dbContext.SaveChangesAsync();

            await _chessHub.Clients.User(otherPlayer?.UserName).ReceiveFold(finalBet);
        }

        public async Task RaiseAsync(Guid matchId, decimal newAmount)
        {
            var match = await _dbContext.Matches.Include(m => m.MatchPlayers)
                .SingleAsync(m => m.Id == matchId);

            var currentPlayer = match.MatchPlayers.SingleOrDefault(p => p.IsBetting);

            if (currentPlayer.UserName != _requestContext.UserName)
                throw new BadRequestException("Wrong user tried to bet, it's not their turn.");

            var otherPlayer = match.MatchPlayers.SingleOrDefault(p => !p.IsBetting);

            currentPlayer.CurrentBet = newAmount;
            currentPlayer.IsBetting = false;
            otherPlayer.IsBetting = true;

            await _dbContext.SaveChangesAsync();

            await _chessHub.Clients.User(otherPlayer?.UserName).ReceiveBet(newAmount);
        }
    }
}
