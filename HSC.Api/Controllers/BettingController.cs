using HSC.Bll.BettingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BettingController : ControllerBase
    {
        private readonly IBettingService _bettingService;

        public BettingController(IBettingService bettingService)
        {
            _bettingService = bettingService;
        }

        [HttpPost]
        public Task CheckAsync(Guid matchId, string userName)
        {
            return _bettingService.CheckAsync(matchId, userName);
        }

        [HttpPost]
        public Task CallAsnyc(Guid matchId, string userName)
        {
            return _bettingService.CallAsnyc(matchId, userName);
        }

        [HttpPost]
        public Task RaiseAsync(Guid matchId, string userName, decimal newAmount)
        {
            return _bettingService.RaiseAsync(matchId, userName, newAmount);
        }

        [HttpPost]
        public Task FoldAsync(Guid matchId, string userName)
        {
            return _bettingService.FoldAsync(matchId, userName);
        }
    }
}
