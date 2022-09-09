using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Bll.TournamentJobService;
using HSC.Bll.TournamentService;
using Quartz;

namespace HSC.Bll.Scheduling.Jobs
{
    public class EndTournamentJob : IJob
    {
        private readonly ITournamentJobService _tournamentJobService;

        public EndTournamentJob(ITournamentJobService tournamentJobService)
        {
            _tournamentJobService = tournamentJobService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var tournamentId = context.JobDetail.JobDataMap.GetIntValue("TournamentId");
            await _tournamentJobService.TournamentOver(tournamentId);
        }
    }
}
