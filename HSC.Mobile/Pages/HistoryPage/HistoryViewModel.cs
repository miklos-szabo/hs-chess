using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Resources.Translation;
using HSCApi;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.HistoryPage
{
    public class HistoryViewModel: BaseViewModel
    {
        private readonly HistoryService _historyService;
        private readonly IStringLocalizer<Translation> _localizer;
        private string _opponent;
        private SearchSimpleResult _searchSimpleResult;
        private DateTimeOffset? _intervalStart = new DateTimeOffset(2021, 01, 01, 0, 0, 0, TimeSpan.Zero);
        private DateTimeOffset? _intervalEnd = new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero);
        private List<PastGameDto> _pastGames;

        public List<PastGameDto> PastGames
        {
            get => _pastGames;
            set => SetField(ref _pastGames, value);
        }

        public IReadOnlyList<string> Results { get; }

        public ICommand SearchCommand { get; set; }
        public ICommand ReviewCommand { get; set; }

        public HistoryViewModel(HistoryService historyService, IStringLocalizer<Translation> localizer)
        {
            _historyService = historyService;
            _localizer = localizer;

            Results = Enum.GetNames(typeof(SearchSimpleResult)).Select(n => _localizer[$"Result.{n}"].Value).ToList();

            Task.Run(async () => await Search());

            SearchCommand = new Command(async () => await Search());
            ReviewCommand = new Command<Guid>(Review);
        }

        public async Task Search()
        {
            PastGames = (await _historyService.GetPastGamesAsync(new HistorySearchDto
            {
                IntervalEnd = IntervalEnd,
                IntervalStart = IntervalStart,
                Opponent = Opponent,
                SearchSimpleResult = SearchSimpleResult
            }, 30, 0)).ToList();
        }

        public void Review(Guid matchId)
        {
            Console.WriteLine(matchId);
        }

        public string Opponent
        {
            get => _opponent;
            set => SetField(ref _opponent, value);
        }

        public SearchSimpleResult SearchSimpleResult
        {
            get => _searchSimpleResult;
            set => SetField(ref _searchSimpleResult, value);
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
    }
}
