using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using HSC.Mobile.Resources.Translation;
using HSC.Mobile.Services;
using HSCApi;
using Java.Security;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.TournamentsPage.TournamentDetails
{
    public class TournamentDetailsViewModel: BaseViewModel
    {
        private readonly SignalrService _signalrService;
        private readonly AlertService _alertService;
        private readonly TournamentService _tournamentService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IStringLocalizer<Translation> _localizer;
        private readonly MatchService _matchService;
        private readonly CurrentGameService _currentGameService;
        private readonly AuthService _authService;
        private readonly NavigationService _navigationService;

        public TimeSpan CountdownTo
        {
            get => _countdownTo;
            set => SetField(ref _countdownTo, value);
        }

        public bool HasFinished
        {
            get => _hasFinished;
            set => SetField(ref _hasFinished, value);
        }

        public bool HasStarted
        {
            get => _hasStarted;
            set => SetField(ref _hasStarted, value);
        }

        private int _tournamentId;
        private TournamentDetailsDto _details;
        private bool _isSearching;
        private ObservableCollection<TournamentPlayerDto> _standings;

        private IDispatcherTimer _countdownTimer;
        private TimeSpan _countdownTo;
        private bool _hasFinished;
        private bool _hasStarted;

        public ObservableCollection<TournamentPlayerDto> Standings
        {
            get => _standings;
            set => SetField(ref _standings, value);
        }

        public bool IsSearching
        {
            get => _isSearching;
            set => SetField(ref _isSearching, value);
        }

        public TournamentDetailsDto Details
        {
            get => _details;
            set => SetField(ref _details, value);
        }

        public ICommand JoinCommand { get; set; }
        public ICommand FindMatchCommand { get; set; }

        public TournamentDetailsViewModel(SignalrService signalrService, AlertService alertService, TournamentService tournamentService, IServiceProvider serviceProvider, IStringLocalizer<Translation> localizer, MatchService matchService, CurrentGameService currentGameService, AuthService authService, NavigationService navigationService)
        {
            _signalrService = signalrService;
            _alertService = alertService;
            _tournamentService = tournamentService;
            _serviceProvider = serviceProvider;
            _localizer = localizer;
            _matchService = matchService;
            _currentGameService = currentGameService;
            _authService = authService;
            _navigationService = navigationService;

            _signalrService.MatchFoundEvent += MatchFound;
            _signalrService.TournamentStartedEvent += TournamentStarted;
            _signalrService.TournamentOverEvent += TournamentOver;
            _signalrService.UpdateStandingsEvent += UpdateStandings;

            _countdownTimer = Application.Current.Dispatcher.CreateTimer();
            _countdownTimer.Interval = TimeSpan.FromSeconds(1);
            _countdownTimer.Tick += (sender, args) =>
            {
                CountdownTo = CountdownTo.Subtract(TimeSpan.FromSeconds(1));
            };

            JoinCommand = new Command(async () => await JoinTournament());
            FindMatchCommand = new Command(async () => await FindMatch());
        }

        private async void UpdateStandings(object sender, EventArgs e)
        {
            await GetStandings();
        }

        private async void TournamentOver(object sender, TournamentOverDto e)
        {
            await GetDetails();
        }

        private async void TournamentStarted(object sender, EventArgs e)
        {
            await GetDetails();
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

        public async Task GetDetails()
        {
            Details = await _tournamentService.GetTournamentDetailsAsync(TournamentId);
            CountdownTo = Details.TournamentStatus == TournamentStatus.NotStarted ? Details.StartTime.Subtract(DateTime.UtcNow) : Details.StartTime.Add(Details.Length).Subtract(DateTime.UtcNow);
            CountdownTo = TimeSpan.FromSeconds(Math.Round(CountdownTo.TotalSeconds));

            if (Details.TournamentStatus != TournamentStatus.Finished)
            {
                _countdownTimer.Start();
            }

            HasStarted = Details.TournamentStatus == TournamentStatus.Ongoing;
            HasFinished = Details.TournamentStatus == TournamentStatus.Finished;
        }

        public async Task GetStandings()
        {
            Standings = (await _tournamentService.GetStandingsAsync(TournamentId)).ToObservableCollection();
        }

        public async Task JoinTournament()
        {
            await _tournamentService.JoinTournamentAsync(TournamentId);
            await GetDetails();
            await _alertService.DisplayInfoNoti(_localizer["Tournaments.Joined"]);
        }

        public async Task FindMatch()
        {
            IsSearching = true;
            await _tournamentService.SearchForNextMatchAsync(TournamentId);
        }

        public int TournamentId
        {
            get => _tournamentId;
            set
            {
                SetField(ref _tournamentId, value);
                Task.Run(async () => await GetDetails());
                Task.Run(async () => await GetStandings());
            }
        }
    }
}
