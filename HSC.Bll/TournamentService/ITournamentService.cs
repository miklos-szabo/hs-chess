using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Transfer.Tournament;

namespace HSC.Bll.TournamentService
{
    public interface ITournamentService
    {
        Task CreateTournamentAsync(CreateTournamentDto dto);
        Task<List<TournamentListDto>> GetTournamentsAsync(SearchTournamentDto dto);
        Task<TournamentDetailsDto> GetTournamentDetailsAsync(int id);
        Task JoinTournamentAsync(int id);
        Task TournamentOver(int id);
        Task TournamentStart(int id);
        Task<List<TournamentMessageDto>> GetMessages(int id);
        Task SendMessage(int id, string message);
        Task SearchForNextMatch(int id);
    }
}
