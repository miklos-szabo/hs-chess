using HSC.Mobile.Pages.CashierPage;
using HSC.Mobile.Pages.CustomGamesPage;
using HSC.Mobile.Pages.FlyoutPage;
using HSC.Mobile.Pages.FriendsPage;
using HSC.Mobile.Pages.GroupsPage;
using HSC.Mobile.Pages.HistoryPage;
using HSC.Mobile.Pages.QuickMatchPage;
using HSC.Mobile.Pages.Settings;
using HSC.Mobile.Pages.TournamentsPage;
using HSC.Mobile.Services;

namespace HSC.Mobile
{
    public partial class MainPage : FlyoutPage
    {
        private readonly NavigationService _navigationService;
        private readonly IServiceProvider _serviceProvider;
        private readonly HscFlyoutPage _flyoutPage;


        public MainPage(NavigationService navigationService, HscFlyoutPage flyoutPage, IServiceProvider serviceProvider)
        {
            _navigationService = navigationService;
            _flyoutPage = flyoutPage;
            _serviceProvider = serviceProvider;

            Flyout = _flyoutPage;
            Detail = new NavigationPage(_serviceProvider.GetService<QuickMatchPage>());
            InitializeComponent();

            _flyoutPage.navigation.SelectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.CurrentSelection.FirstOrDefault() as FlyoutPageItem;
            
            if (item != null)
            {
                switch (item.TargetType)
                {
                    case { } cp when cp == typeof(CashierPage):
                        _navigationService.ChangeDetailPage(_serviceProvider.GetService<CashierPage>());
                        break;
                    case { } cp when cp == typeof(CustomGamesPage):
                        _navigationService.ChangeDetailPage(_serviceProvider.GetService<CustomGamesPage>());
                        break;
                    case { } cp when cp == typeof(FriendsPage):
                        _navigationService.ChangeDetailPage(_serviceProvider.GetService<FriendsPage>());
                        break;
                    case { } cp when cp == typeof(GroupsPage):
                        _navigationService.ChangeDetailPage(_serviceProvider.GetService<GroupsPage>());
                        break;
                    case { } cp when cp == typeof(HistoryPage):
                        _navigationService.ChangeDetailPage(_serviceProvider.GetService<HistoryPage>());
                        break;
                    case { } cp when cp == typeof(QuickMatchPage):
                        _navigationService.ChangeDetailPage(_serviceProvider.GetService<QuickMatchPage>());
                        break;
                    case { } cp when cp == typeof(SettingsPage):
                        _navigationService.ChangeDetailPage(_serviceProvider.GetService<SettingsPage>());
                        break;
                    case { } cp when cp == typeof(TournamentsPage):
                        _navigationService.ChangeDetailPage(_serviceProvider.GetService<TournamentsPage>());
                        break;
                }

                IsPresented = false;
            }
        }
    }
}