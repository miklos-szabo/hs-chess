using HSC.Bll.BettingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class BettingController : ControllerBase
    {
        private readonly IBettingService _bettingService;

        public BettingController(IBettingService bettingService)
        {
            _bettingService = bettingService;
        }

        [HttpPost]
        public Task CheckAsync(Guid matchId)
        {
            return _bettingService.CheckAsync(matchId);
        }

        [HttpPost]
        public Task CallAsnyc(Guid matchId)
        {
            return _bettingService.CallAsnyc(matchId);
        }

        [HttpPost]
        public Task RaiseAsync(Guid matchId, decimal newAmount)
        {
            return _bettingService.RaiseAsync(matchId, newAmount);
        }

        [HttpPost]
        public Task FoldAsync(Guid matchId)
        {
            return _bettingService.FoldAsync(matchId);
        }
    }
}
