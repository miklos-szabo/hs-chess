using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Analysis
{
    public class GameToBeAnalysedDto
    {
        public Guid MatchId { get; set; }
        public List<string> Fens { get; set; }
    }
}
