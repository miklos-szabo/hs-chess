using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Bll.TournamentService;
using Quartz;

namespace HSC.Bll.Scheduling
{
    public class EndTournamentJob: IJob
    {
        private readonly ITournamentService _tournamentService;

        public EndTournamentJob(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var tournamentId = context.JobDetail.JobDataMap.GetIntValue("TournamentId");
            await _tournamentService.TournamentOver(tournamentId);
        }
    }
}
