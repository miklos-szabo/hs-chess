using HSC.Bll.MatchFinderService;
using HSC.Transfer.Searching;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MatchFinderController : ControllerBase
    {
        private readonly IMatchFinderService _matchFinderService;

        public MatchFinderController(IMatchFinderService matchFinderService)
        {
            _matchFinderService = matchFinderService;
        }

        [HttpPost]
        public Task SearchForMatchAsync(SearchingForMatchDto dto)
        {
            return _matchFinderService.SearchForMatchAsync(dto);
        }

        [HttpGet]
        public Task<List<CustomGameDto>> GetCustomGamesAsync()
        {
            return _matchFinderService.GetCustomGamesAsync();
        }

        [HttpPost]
        public Task CreateCustomGameAsync(CreateCustomGameDto dto)
        {
            return _matchFinderService.CreateCustomGameAsync(dto);
        }

        [HttpPost]
        public Task<Guid> JoinCustomGame(int challengeId)
        {
            return _matchFinderService.JoinCustomGame(challengeId);
        }
    }
}
