using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Pages.ChessPage;
using HSCApi;
using PonzianiComponents;

namespace HSC.Mobile.Services
{
    public class EventService
    {
        public event EventHandler<MovePlayedInfo> OwnMovePlayed;
        public event EventHandler<HistoryMove> OpponentMoveProcessed;
        public event EventHandler<Result> OwnMoveEndedGame;
        public event EventHandler<string> ChangeToFen;

        public void OnOwnMovePlayed(MovePlayedInfo mpi)
        {
            OwnMovePlayed?.Invoke(this, mpi);
        }

        public void OnOpponentMoveProcessed(HistoryMove move)
        {
            OpponentMoveProcessed?.Invoke(this, move);
        }

        public void OnOwnMoveEndedGame(Result r)
        {
            OwnMoveEndedGame?.Invoke(this, r);
        }

        public void OnChangeToFen(string fen)
        {
            ChangeToFen?.Invoke(this, fen);
        }
    }
}
