using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Resources.Translation;
using HSC.Mobile.Services;
using HSCApi;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.FlyoutPage
{
    public class HscFlyoutPageViewModel: BaseViewModel
    {
        public ICommand LogoutCommand { get; set; }
        private readonly AccountService _accountService;
        private readonly IServiceProvider _provider;
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;
        private UserMenuDto _userData;
        private readonly SignalrService _signalrService;
        private readonly MatchService _matchService;
        private readonly AlertService _alertService;
        private readonly IStringLocalizer<Translation> _localizer;
        private readonly MatchFinderService _matchFinderService;
        private readonly CurrentGameService _currentGameService;

        public UserMenuDto UserData
        {
            get => _userData;
            set => SetField(ref _userData, value);
        }

        public HscFlyoutPageViewModel(AccountService accountService, IServiceProvider provider, AuthService authService, NavigationService navigationService, SignalrService signalrService, MatchService matchService, AlertService alertService, IStringLocalizer<Translation> localizer, MatchFinderService matchFinderService, CurrentGameService currentGameService)
        {
            _accountService = accountService;
            _provider = provider;
            _authService = authService;
            _navigationService = navigationService;
            _signalrService = signalrService;
            _matchService = matchService;
            _alertService = alertService;
            _localizer = localizer;
            _matchFinderService = matchFinderService;
            _currentGameService = currentGameService;

            LogoutCommand = new Command(Logout);

            Task.Run(async () => await _signalrService.Connect());
            Task.Run(async () => await GetUserData());

            _signalrService.FriendRequestReceivedEvent += FriendRequestReceived;
            _signalrService.ChallengeFoundEvent += ChallangeReceived;
        }

        private async void ChallangeReceived(object sender, ChallengeDto e)
        {
            await _alertService.DisplayInfoActionNoti(_localizer["Challenge.Text"] + " " + e.UserName, _localizer["Challenge.Button"], (
                async () =>
                {
                    _signalrService.MatchFoundEvent += MatchFound;
                    await _matchFinderService.JoinCustomGameAsync(e.Id);
                }));
        }

        private async void MatchFound(object sender, Guid matchId)
        {
            _signalrService.MatchFoundEvent -= MatchFound;
            var fullData = await _matchService.GetMatchDataAsync(matchId);
            _currentGameService.SetData(_authService.UserName, matchId, fullData);

            var chessPage = _provider.GetService<ChessPageView>();
            _navigationService.ChangeDetailPage(chessPage);
        }

        private async void FriendRequestReceived(object sender, string user)
        {
            await _alertService.DisplayInfoNoti(_localizer["FriendRequestReceived"] + " " + user);
        }

        public async Task GetUserData()
        {
            UserData = await _accountService.GetUserMenuDataAsync();
            _authService.UserName = UserData.UserName;
        }

        public void Logout()
        {
            _authService.Logout();
            _signalrService.Disconnect();
            _navigationService.ChangeMainPage(_provider.GetService<AuthenticationPage.AuthenticationPage>());
        }
    }
}
