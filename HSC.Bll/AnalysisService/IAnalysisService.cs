using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Transfer.Analysis;

namespace HSC.Bll.AnalysisService
{
    public interface IAnalysisService
    {
        Task<AnalysedGameDto> GetAnalysis(GameToBeAnalysedDto dto);
    }
}
