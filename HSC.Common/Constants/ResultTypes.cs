using HSC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Common.Constants
{
    public static class ResultTypes
    {
        public static readonly Result[] WhiteWon =
        {
            Result.WhiteWonByCheckmate,
            Result.WhiteWonByResignation,
            Result.WhiteWonByTimeout
        };

        public static readonly Result[] BlackWon =
{
            Result.BlackWonByCheckmate,
            Result.BlackWonByResignation,
            Result.BlackWonByTimeOut
        };

        public static readonly Result[] Draw =
        {
            Result.DrawByStalemate,
            Result.DrawByInsufficientMaterial,
            Result.DrawByTimeoutVsInsufficientMaterial,
            Result.DrawByAgreement,
            Result.DrawByThreefoldRepetition
        };
    }
}
