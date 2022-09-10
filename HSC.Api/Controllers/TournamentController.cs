using HSC.Bll.TournamentService;
using HSC.Transfer.Tournament;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HSC.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        [HttpPost]
        public async Task CreateTournamentAsync(CreateTournamentDto dto)
        {
            await _tournamentService.CreateTournamentAsync(dto);
        }

        [HttpPost]
        public async Task<List<TournamentListDto>> GetTournamentsAsync(SearchTournamentDto dto)
        {
            return await _tournamentService.GetTournamentsAsync(dto);
        }

        [HttpGet("{id}")]
        public async Task<TournamentDetailsDto> GetTournamentDetailsAsync(int id)
        {
            return await _tournamentService.GetTournamentDetailsAsync(id);
        }

        [HttpPost("{id}")]
        public async Task JoinTournamentAsync(int id)
        {
            await _tournamentService.JoinTournamentAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<List<TournamentMessageDto>> GetMessages(int id)
        {
            return await _tournamentService.GetMessages(id);
        }

        [HttpPost("{id}")]
        public async Task SendMessage(int id, string message)
        {
            await _tournamentService.SendMessage(id, message);
        }

        [HttpPost("{id}")]
        public async Task SearchForNextMatch(int id)
        {
            await _tournamentService.SearchForNextMatch(id);
        }

        [HttpGet("{id}")]
        public async Task<List<TournamentPlayerDto>> GetStandingsAsync(int id)
        {
            return await _tournamentService.GetStandingsAsync(id);
        }
    }
}
