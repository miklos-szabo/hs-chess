using HSC.Mobile.Enums;
using PonzianiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Pages.ChessPage
{
    public class ChessPageViewModel: BaseViewModel
    {
        private int _moveCount = 0;
        private string _lastMove;

        public ChessPageViewModel()
        {
            MessagingCenter.Subscribe<ChessBoardPage.ChessBoardPage, MovePlayedInfo>(this, MessageTypes.MoveMade, (_, arg) => MoveMade(arg));
        }

        private void MoveMade(MovePlayedInfo e)
        {
            MoveCount++;
            LastMove = e.San;
        }

        public int MoveCount
        {
            get => _moveCount;
            set => SetField(ref _moveCount, value);
        }

        public string LastMove
        {
            get => _lastMove;
            set => SetField(ref _lastMove, value);
        }
    }
}
