using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Common.Enums
{
    public enum Result
    {
        Ongoing,
        WhiteWonByTimeout,
        WhiteWonByCheckmate,
        WhiteWonByResignation,
        BlackWonByTimeOut,
        BlackWonByCheckmate,
        BlackWonByResignation,
        DrawByInsufficientMaterial,
        DrawByStalemate,
        DrawByAgreement,
        DrawByTimeoutVsInsufficientMaterial,
        DrawByThreefoldRepetition,
    }
}
