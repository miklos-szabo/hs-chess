using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Dal.Entities
{
    public class Analysis
    {
        public int Id { get; set; }
        public Guid MatchId { get; set; }
        public string AnalysedGame { get; set; }

        public virtual Match Match { get; set; }
    }
}
