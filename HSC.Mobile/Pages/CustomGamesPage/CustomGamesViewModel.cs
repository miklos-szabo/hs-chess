using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using HSC.Mobile.Services;
using HSCApi;

namespace HSC.Mobile.Pages.CustomGamesPage
{
    public class CustomGamesViewModel: BaseViewModel
    {
        private readonly MatchFinderService _matchFinderService;
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationService _navigationService;
        private readonly CurrentGameService _currentGameService;
        private readonly SignalrService _signalrService;
        private readonly MatchService _matchService;
        private readonly AuthService _authService;
        private ObservableCollection<CustomGameDto> _customGames = new ObservableCollection<CustomGameDto>();

        public ICommand JoinGameCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public CustomGamesViewModel(MatchFinderService matchFinderService, NavigationService navigationService, IServiceProvider serviceProvider, CurrentGameService currentGameService, SignalrService signalrService, MatchService matchService, AuthService authService)
        {
            _matchFinderService = matchFinderService;
            _navigationService = navigationService;
            _serviceProvider = serviceProvider;
            _currentGameService = currentGameService;
            _signalrService = signalrService;
            _matchService = matchService;
            _authService = authService;

            JoinGameCommand = new Command<int>(async id => await JoinGame(id));
            RefreshCommand = new Command(async () => await GetCustomGames());

            Task.Run(async () => await GetCustomGames());
        }

        public async Task JoinGame(int Id)
        {
            _signalrService.MatchFoundEvent += MatchFound;

            await _matchFinderService.JoinCustomGameAsync(Id);
        }

        public async Task GetCustomGames()
        {
            CustomGames = (await _matchFinderService.GetCustomGamesAsync()).ToObservableCollection();
        }

        private async void MatchFound(object sender, Guid matchId)
        {
            _signalrService.MatchFoundEvent -= MatchFound;

            var fullData = await _matchService.GetMatchDataAsync(matchId);
            _currentGameService.SetData(_authService.UserName, matchId, fullData);

            var chessPage = _serviceProvider.GetService<ChessPageView>();
            _navigationService.ChangeDetailPage(chessPage);
        }

        public ObservableCollection<CustomGameDto> CustomGames
        {
            get => _customGames;
            set => SetField(ref _customGames, value);
        }
    }
}
