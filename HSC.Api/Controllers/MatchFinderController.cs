using HSC.Bll.MatchFinderService;
using HSC.Transfer.Searching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MatchFinderController : ControllerBase
    {
        private readonly IMatchFinderService _matchFinderService;

        public MatchFinderController(IMatchFinderService matchFinderService)
        {
            _matchFinderService = matchFinderService;
        }

        [HttpPost]
        public async Task SearchForMatchAsync(SearchingForMatchDto dto)
        {
            await _matchFinderService.SearchForMatchAsync(dto);
        }

        [HttpGet]
        public async Task<List<CustomGameDto>> GetCustomGamesAsync()
        {
            return await _matchFinderService.GetCustomGamesAsync();
        }

        [HttpPost]
        public async Task CreateCustomGameAsync(CreateCustomGameDto dto)
        {
            await _matchFinderService.CreateCustomGameAsync(dto);
        }

        [HttpPost]
        public async Task JoinCustomGame(int challengeId)
        {
            await _matchFinderService.JoinCustomGame(challengeId);
        }
    }
}
