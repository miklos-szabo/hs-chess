using HSC.Bll.HistoryService;
using HSC.Transfer.History;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;

        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpPost]
        public async Task<List<PastGameDto>> GetPastGamesAsync([FromBody]HistorySearchDto searchDto, int pageSize, int page)
        {
            return await _historyService.GetPastGamesAsync(searchDto, pageSize, page);
        }
    }
}
