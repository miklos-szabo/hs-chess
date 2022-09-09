using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.TournamentJobService
{
    public interface ITournamentJobService
    {
        Task TournamentOver(int id);
        Task TournamentStart(int id);
    }
}
