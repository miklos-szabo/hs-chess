using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.Analysis
{
    public class BestMovesDto
    {
        public int MoveNumber { get; set; }
        public EngineLineDto FirstBest { get; set; }
        public EngineLineDto SecondBest { get; set; }
        public EngineLineDto ThirdBest { get; set; }
    }
}
