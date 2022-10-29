using HSC.Bll.AnalysisService;
using HSC.Transfer.Analysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpPost]
        public async Task<AnalysedGameDto> GetAnalysis([FromBody] GameToBeAnalysedDto dto)
        {
            return await _analysisService.GetAnalysis(dto);
        }
    }
}
