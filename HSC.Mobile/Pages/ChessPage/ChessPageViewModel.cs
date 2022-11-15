using HSC.Mobile.Enums;
using Keycloak.Net.Models.ClientInitialAccess;
using Keycloak.Net;
using PonzianiComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSCApi;

namespace HSC.Mobile.Pages.ChessPage
{
    public class ChessPageViewModel: BaseViewModel
    {
        private int _moveCount = 0;
        private string _lastMove;
        private readonly AccountService _accountService;

        public ChessPageViewModel(AccountService accountService)
        {
            _accountService = accountService;
            MessagingCenter.Subscribe<ChessBoardPage.ChessBoardPage, MovePlayedInfo>(this, MessageTypes.MoveMade, (_, arg) => MoveMade(arg));
        }

        private async Task MoveMade(MovePlayedInfo e)
        {
            MoveCount++;
            LastMove = e.San;

            var x = await _accountService.GetFullUserDataAsync();

            //var x = new KeycloakClient("https://hsckeycloak13.fagwgranamc5c8bp.westeurope.azurecontainer.io:8443", "user1", "user1");
            //var ahh = x.CreateInitialAccessTokenAsync("chess", new ClientInitialAccessCreatePresentation());
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
