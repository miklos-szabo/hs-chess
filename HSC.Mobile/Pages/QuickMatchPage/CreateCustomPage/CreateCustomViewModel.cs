using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Services;
using HSCApi;

namespace HSC.Mobile.Pages.QuickMatchPage.CreateCustomPage
{
    public class CreateCustomViewModel: BaseViewModel
    {
        private int _minutes;
        private int _increment;
        private decimal _minimumBet;
        private decimal _maximumBet;

        private readonly MatchFinderService _matchFinderService;
        private readonly AuthService _authService;
        private readonly SignalrService _signalrService;
        private readonly MatchService _matchService;
        private readonly CurrentGameService _currentGameService;
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationService _navigationService;
        private bool _isSearching;
        private ICommand _createCommand;

        public string ToUserName { get; set; }

        public ICommand CreateCommand
        {
            get => _createCommand;
            set => SetField(ref _createCommand, value);
        }

        public CreateCustomViewModel(MatchFinderService matchFinderService, AuthService authService, SignalrService signalrService, MatchService matchService, CurrentGameService currentGameService, IServiceProvider serviceProvider, NavigationService navigationService)
        {
            _matchFinderService = matchFinderService;
            _authService = authService;
            _signalrService = signalrService;
            _matchService = matchService;
            _currentGameService = currentGameService;
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;

            CreateCommand = new Command(async () => await CreateCustomGame());
        }

        public async Task CreateCustomGame()
        {
            IsSearching = true;
            _signalrService.MatchFoundEvent += MatchFound;
            await _matchFinderService.CreateCustomGameAsync(new CreateCustomGameDto
            {
                Increment = Increment,
                MaximumBet = MaximumBet,
                MinimumBet = MinimumBet,
                TimeLimitMinutes = Minutes,
                UserName = string.IsNullOrEmpty(ToUserName) ? null : ToUserName,
            });
        }

        private async void MatchFound(object sender, Guid matchId)
        {
            IsSearching = false;
            _signalrService.MatchFoundEvent -= MatchFound;

            var fullData = await _matchService.GetMatchDataAsync(matchId);
            _currentGameService.SetData(_authService.UserName, matchId, fullData);

            var chessPage = _serviceProvider.GetService<ChessPageView>();
            _navigationService.ChangeDetailPage(chessPage);
        }

        public int Minutes
        {
            get => _minutes;
            set => SetField(ref _minutes, value);
        }

        public int Increment
        {
            get => _increment;
            set => SetField(ref _increment, value);
        }

        public decimal MinimumBet
        {
            get => _minimumBet;
            set => SetField(ref _minimumBet, value);
        }

        public decimal MaximumBet
        {
            get => _maximumBet;
            set => SetField(ref _maximumBet, value);
        }

        public bool IsSearching
        {
            get => _isSearching;
            set => SetField(ref _isSearching, value);
        }
    }
}
