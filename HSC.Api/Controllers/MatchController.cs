using HSC.Bll.Match;
using HSC.Transfer.Match;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
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
    }
}
