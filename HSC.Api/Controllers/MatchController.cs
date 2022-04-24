using HSC.Bll.Match;
using HSC.Common.Enums;
using HSC.Transfer.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("{matchId}")]
        public async Task<MatchStartDto> GetMatchStartingDataAsync(Guid matchId)
        {
            return await _matchService.GetMatchStartingDataAsync(matchId);
        }

        [HttpPost("{matchId}")]
        public async Task MatchOver(Guid matchId, Result result, string winnerUserName)
        {
            await _matchService.MatchOver(matchId, result, winnerUserName);
        }

        [HttpPost("{matchId}")]
        public async Task SaveMatchPgn(Guid matchId, string pgn)
        {
            await _matchService.SaveMatchPgn(matchId, pgn);
        }

        [HttpGet("{matchId}")]
        public async Task<string> GetMatchPgn(Guid matchId)
        {
            return await _matchService.GetMatchPgn(matchId);
        }
    }
}
