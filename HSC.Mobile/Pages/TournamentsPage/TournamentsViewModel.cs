using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using HSC.Mobile.Pages.TournamentsPage.TournamentDetails;
using HSC.Mobile.Resources.Translation;
using HSC.Mobile.Services;
using HSCApi;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.TournamentsPage
{
    public class TournamentsViewModel: BaseViewModel
    {
        private readonly NavigationService _navigationService;
        private readonly TournamentService _tournamentService;
        private readonly IServiceProvider _serviceProvider;
        private string _titleSearch;
        private DateTime? _startInterval;
        private DateTime? _endInterval;
        private decimal? _buyInMinimum;
        private decimal? _buyInMaximum;
        private bool _finished;
        private ObservableCollection<TournamentListDto> _tournaments = new ObservableCollection<TournamentListDto>();

        public ICommand SearchCommand { get; set; }
        public ICommand DetailsCommand { get; set; }

        public TournamentsViewModel(IServiceProvider serviceProvider, NavigationService navigationService, TournamentService tournamentService)
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _tournamentService = tournamentService;

            SearchCommand = new Command(async () => await GetTournaments());
            DetailsCommand = new Command<int>(async id => await Details(id));

            Task.Run(async () => await GetTournaments());
        }

        public async Task GetTournaments()
        {
            Tournaments = (await _tournamentService.GetTournamentsAsync(new SearchTournamentDto
            {
                Title = TitleSearch,
                PastTournaments = Finished,
                StartDateIntervalStart = StartInterval,
                StartDateIntervalEnd = EndInterval,
                BuyInMin = BuyInMinimum,
                BuyInMax = BuyInMaximum,
            })).ToObservableCollection();
        }

        public async Task Details(int id)
        {
            var page = _serviceProvider.GetService<TournamentDetailsPage>();
            page.ViewModel.TournamentId = id;
            await _navigationService.PushAsync(page);
        }


        public string TitleSearch
        {
            get => _titleSearch;
            set => SetField(ref _titleSearch, value);
        }

        public DateTime? StartInterval
        {
            get => _startInterval;
            set => SetField(ref _startInterval, value);
        }

        public DateTime? EndInterval
        {
            get => _endInterval;
            set => SetField(ref _endInterval, value);
        }

        public decimal? BuyInMinimum
        {
            get => _buyInMinimum;
            set => SetField(ref _buyInMinimum, value);
        }

        public decimal? BuyInMaximum
        {
            get => _buyInMaximum;
            set => SetField(ref _buyInMaximum, value);
        }

        public bool Finished
        {
            get => _finished;
            set => SetField(ref _finished, value);
        }

        public ObservableCollection<TournamentListDto> Tournaments
        {
            get => _tournaments;
            set => SetField(ref _tournaments, value);
        }
    }
}
