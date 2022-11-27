using HSC.Mobile.Enums;
using PonzianiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Services;
using HSCApi;

namespace HSC.Mobile.Pages.ChessPage
{
    public class ChessPageViewModel: BaseViewModel
    {
        public Guid MatchId { get; set; }
        private int _moveCount = 0;
        private string _lastMove;
        private readonly AccountService _accountService;
        private readonly SignalrService _signalrService;

        public ChessPageViewModel(AccountService accountService, SignalrService signalrService)
        {
            _accountService = accountService;
            _signalrService = signalrService;
            MessagingCenter.Subscribe<ChessBoardPage.ChessBoardPage, MovePlayedInfo>(this, MessageTypes.MoveMade, (_, arg) => MoveMade(arg));

            signalrService.MoveReceivedEvent += (sender, move) =>
            {
                // Make move
            };
        }

        private async Task MoveMade(MovePlayedInfo e)
        {
            MoveCount++;
            LastMove = e.San;

            var x = await _accountService.GetFullUserDataAsync();
            MoveCount++;
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
