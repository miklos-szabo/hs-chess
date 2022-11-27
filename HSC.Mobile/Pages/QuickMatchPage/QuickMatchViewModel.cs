using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Services;
using HSCApi;

namespace HSC.Mobile.Pages.QuickMatchPage
{
    public class QuickMatchViewModel: BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly MatchFinderService _matchFinderService;
        private readonly NavigationService _navigationService;
        private readonly IServiceProvider _provider;
        private readonly SignalrService _signalrService;
        private bool _isSearching;

        public QMTimeControl SelectedTimeControl { get; set; }
        public QMBet SelectedBet { get; set; }

        public bool IsSearching
        {
            get => _isSearching;
            set => SetField(ref _isSearching, value);
        }

        public ICommand SearchCommand { get; set; }
        public ICommand CreateCustomCommand { get; set; }

        public QuickMatchViewModel(AuthService authService, MatchFinderService matchFinderService, NavigationService navigationService, IServiceProvider provider, SignalrService signalrService)
        {
            _authService = authService;
            _matchFinderService = matchFinderService;
            _navigationService = navigationService;
            _provider = provider;
            _signalrService = signalrService;

            // () => SelectedBet != null && SelectedTimeControl != null && !IsSearching
            SearchCommand = new Command(async () => await SearchForMatch());

            CreateCustomCommand = new Command(async () => await CreateCustom());
        }

        public async Task SearchForMatch()
        {
            IsSearching = true;
            _signalrService.MatchFoundEvent += MatchFound;

            await _matchFinderService.SearchForMatchAsync(new SearchingForMatchDto
            {
                TimeLimitMinutes = SelectedTimeControl.Minutes,
                Increment = SelectedTimeControl.Increment,
                MinimumBet = SelectedBet.MinimumBet,
                MaximumBet = SelectedBet.MaximumBet,
                UserName = _authService.UserName
            });
        }

        public async Task CreateCustom()
        {
            await _navigationService.PushAsync(_provider.GetService<CreateCustomPage.CreateCustomPage>());
        }

        private void MatchFound(object sender, Guid matchId)
        {
            IsSearching = false;
            _signalrService.MatchFoundEvent -= MatchFound;

            var chessPage = _provider.GetService<ChessPageView>();
            chessPage.ViewModel.MatchId = matchId;
            _navigationService.ChangeDetailPage(chessPage);
            //Task.Run(async () => await _navigationService.PushAsync(_provider.GetService<ChessPageView>()));
        }
    }
}
