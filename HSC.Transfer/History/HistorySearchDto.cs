using HSC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Transfer.History
{
    public class HistorySearchDto
    {
        public string Opponent { get; set; }
        public Result Result { get; set; }
        public DateTimeOffset? IntervalStart { get; set; }
        public DateTimeOffset? IntervalEnd { get; set; }
    }
}
