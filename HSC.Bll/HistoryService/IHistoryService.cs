using HSC.Transfer.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Bll.HistoryService
{
    public interface IHistoryService
    {
        Task<List<PastGameDto>> GetPastGamesAsync(HistorySearchDto searchDto, int pageSize, int page);
    }
}
