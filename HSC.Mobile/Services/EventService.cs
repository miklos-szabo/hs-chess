using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Pages.ChessPage;
using PonzianiComponents;
using PonzianiComponents.Chesslib;
using Result = HSCApi.Result;

namespace HSC.Mobile.Services
{
    public class EventService
    {
        public event EventHandler<MovePlayedInfo> OwnMovePlayed;
        public event EventHandler<HistoryMove> OpponentMoveProcessed;
        public event EventHandler<Result> OwnMoveEndedGame;
        public event EventHandler<string> ChangeToFen;
        public event EventHandler<int> ScrollToMove;

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

        public void OnScrollToMove(int index)
        {
            ScrollToMove?.Invoke(this, index);
        }
    }
}
