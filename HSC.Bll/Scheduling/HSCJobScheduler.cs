using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Bll.Scheduling.Jobs;
using Microsoft.Extensions.Logging;
using Quartz;

namespace HSC.Bll.Scheduling
{
    public class HSCJobScheduler
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ILogger<HSCJobScheduler> _logger;

        public HSCJobScheduler(ISchedulerFactory schedulerFactory, ILogger<HSCJobScheduler> logger)
        {
            _schedulerFactory = schedulerFactory;
            _logger = logger;
        }

        public async Task RegisterTournamentStartJobAsync(int tournamentId, DateTimeOffset startTime)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var jobKey = new JobKey($"Tournament start job for tournamentId: {tournamentId}");

            var job = JobBuilder.Create<StartTournamentJob>()
                .WithIdentity(jobKey)
                .UsingJobData("TournamentId", tournamentId)
                .Build();

            var triggerKey = new TriggerKey($"Tournament start trigger for tournamentId: {tournamentId}");

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .StartAt(startTime)
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            _logger.LogInformation("TournamentStartJob scheduled - {tournamentId} {startTime}", tournamentId, startTime);
        }

        public async Task RegisterTournamentEndJobAsync(int tournamentId, DateTimeOffset endTime)
        {
            var scheduler = await _schedulerFactory.GetScheduler();

            var jobKey = new JobKey($"Tournament end job for tournamentId: {tournamentId}");

            var job = JobBuilder.Create<EndTournamentJob>()
                .WithIdentity(jobKey)
                .UsingJobData("TournamentId", tournamentId)
                .Build();

            var triggerKey = new TriggerKey($"Tournament end trigger for tournamentId: {tournamentId}");

            var trigger = TriggerBuilder.Create()
                .WithIdentity(triggerKey)
                .StartAt(endTime)
                .Build();

            await scheduler.ScheduleJob(job, trigger);

            _logger.LogInformation("TournamentEndJob scheduled - {tournamentId} {endTime}", tournamentId, endTime);
        }
    }
}
