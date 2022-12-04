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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.HistoryPage
{
    public class HistoryViewModel: BaseViewModel
    {
        private readonly HistoryService _historyService;
        private readonly IStringLocalizer<Translation> _localizer;
        private readonly NavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly MatchService _matchService;
        private readonly CurrentGameService _currentGameService;
        private readonly AuthService _authService;
        private string _opponent;

        private DateTimeOffset? _intervalStart = new DateTimeOffset(2021, 01, 01, 0, 0, 0, TimeSpan.Zero);
        private DateTimeOffset? _intervalEnd = new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero);
        private int _simpleResultIndex;
        private ObservableCollection<PastGameDto> _pastGames = new ObservableCollection<PastGameDto>();

        public IReadOnlyList<string> Results { get; }

        public ICommand SearchCommand { get; set; }
        public ICommand ReviewCommand { get; set; }

        public HistoryViewModel(HistoryService historyService, IStringLocalizer<Translation> localizer, NavigationService navigationService, MatchService matchService, CurrentGameService currentGameService, AuthService authService, IServiceProvider serviceProvider)
        {
            _historyService = historyService;
            _localizer = localizer;
            _navigationService = navigationService;
            _matchService = matchService;
            _currentGameService = currentGameService;
            _authService = authService;
            _serviceProvider = serviceProvider;

            Results = Enum.GetNames(typeof(SearchSimpleResult)).Select(n => _localizer[$"Result.{n}"].Value).ToList();

            SearchCommand = new Command(async () => await Search());
            ReviewCommand = new Command<Guid>(async (id) => await Review(id));

            Task.Run(async () => await Search());
        }

        public async Task Search()
        {
            PastGames = (await _historyService.GetPastGamesAsync(new HistorySearchDto
            {
                IntervalEnd = IntervalEnd,
                IntervalStart = IntervalStart,
                Opponent = Opponent,
                SearchSimpleResult = (SearchSimpleResult)SimpleResultIndex,
            }, 30, 0)).ToObservableCollection();
        }

        public async Task Review(Guid matchId)
        {
            var fullData = await _matchService.GetMatchDataAsync(matchId);
            _currentGameService.SetData(_authService.UserName, matchId, fullData, true);

            var chessPage = _serviceProvider.GetService<HistoryChessPage.HistoryChessPage>();
            await _navigationService.PushAsync(new NavigationPage(chessPage));
        }

        public string Opponent
        {
            get => _opponent;
            set => SetField(ref _opponent, value);
        }


        public DateTimeOffset? IntervalStart
        {
            get => _intervalStart;
            set => SetField(ref _intervalStart, value);
        }

        public DateTimeOffset? IntervalEnd
        {
            get => _intervalEnd;
            set => SetField(ref _intervalEnd, value);
        }

        public int SimpleResultIndex
        {
            get => _simpleResultIndex;
            set => SetField(ref _simpleResultIndex, value);
        }

        public ObservableCollection<PastGameDto> PastGames
        {
            get => _pastGames;
            set => SetField(ref _pastGames, value);
        }
    }
}
