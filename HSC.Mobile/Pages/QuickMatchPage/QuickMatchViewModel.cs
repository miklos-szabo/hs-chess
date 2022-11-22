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

        public ICommand SearchCommand { get; set; }
        public ICommand createCustomCommand { get; set; }

        public QuickMatchViewModel(AuthService authService, MatchFinderService matchFinderService)
        {
            _authService = authService;
            _matchFinderService = matchFinderService;

            SearchCommand = new Command(async () => await SearchForMatch());
        }

        public async Task SearchForMatch()
        {
            var x = await _matchFinderService.GetCustomGamesAsync();
            Console.WriteLine("hi");
        }
    }
}
