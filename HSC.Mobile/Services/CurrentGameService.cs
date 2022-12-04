using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSCApi;

namespace HSC.Mobile.Services
{
    public class CurrentGameService
    {
        public Guid MatchId { get; set; }
        public bool AmIWhite { get; set; }
        public string OwnUserName { get; set; }
        public string OpponentUserName { get; set; }
        public MatchFullDataDto FullData { get; set; }
        public string Pgn { get; set; } // Set by ChessBoardPage on every move
        public bool IsHistoryMode { get; set; }

        public void SetData(string ownUserName, Guid matchId, MatchFullDataDto fullData, bool ishistoryMode = false)
        {
            MatchId = matchId;
            OwnUserName = ownUserName;
            FullData = fullData;
            AmIWhite = ownUserName == fullData.WhiteUserName;
            OpponentUserName = AmIWhite ? fullData.BlackUserName : fullData.WhiteUserName;
            IsHistoryMode = ishistoryMode;
        }
    }
}
