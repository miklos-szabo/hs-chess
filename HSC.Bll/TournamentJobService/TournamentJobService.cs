using HSC.Common.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Dal;
using Microsoft.Extensions.Logging;

namespace HSC.Bll.TournamentJobService
{
    public class TournamentJobService: ITournamentJobService
    {
        private readonly HSCContext _dbContext;
        private readonly ILogger<TournamentJobService> _logger;

        public TournamentJobService(HSCContext dbContext, ILogger<TournamentJobService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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

            //TODO send signalr stuff that tournament is over

            tournament.WinnerUserName = tournament.Players.Any() ? tournament.Players.OrderBy(p => p.Points).First().UserName : "-";
            tournament.TournamentStatus = TournamentStatus.Finished;
            await _dbContext.SaveChangesAsync();

            _logger.LogDebug("Tournament {name}, id: {id} is over.", tournament.Title, tournament.Id);
        }

        public async Task TournamentStart(int id)
        {
            var tournament = await _dbContext.Tournaments.SingleAsync(t => t.Id == id);

            tournament.TournamentStatus = TournamentStatus.Ongoing;
            await _dbContext.SaveChangesAsync();

            _logger.LogDebug("Tournament {name}, id: {id} has started.", tournament.Title, id);
        }
    }
}
