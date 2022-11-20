using System.ComponentModel;
using System.Runtime.CompilerServices;
using HSC.Mobile.Enums;
using HSC.Mobile.Pages;
using HSC.Mobile.Pages.CashierPage;
using HSC.Mobile.Pages.ChessPage.ChessBoardPage;
using HSC.Mobile.Pages.CustomGamesPage;
using HSC.Mobile.Pages.FlyoutPage;
using HSC.Mobile.Pages.FriendsPage;
using HSC.Mobile.Pages.GroupsPage;
using HSC.Mobile.Pages.HistoryPage;
using HSC.Mobile.Pages.QuickMatchPage;
using HSC.Mobile.Pages.Settings;
using HSC.Mobile.Pages.TournamentsPage;
using Kotlin.Reflect;
using PonzianiComponents;
using static Android.Content.ClipData;

namespace HSC.Mobile
{
    public partial class MainPage : FlyoutPage
    {
        private readonly HscFlyoutPage _flyoutPage;
        private readonly QuickMatchPage _quickMatchPage;
        private readonly CashierPage _cashierPage;
        private readonly CustomGamesPage _customGamesPage;
        private readonly FriendsPage _friendsPage;
        private readonly GroupsPage _groupsPage;
        private readonly HistoryPage _historyPage;
        private readonly SettingsPage _settingsPage;
        private readonly TournamentsPage _tournamentsPage;


        public MainPage(HscFlyoutPage flyoutPage, QuickMatchPage quickMatchPage, CashierPage cashierPage, CustomGamesPage customGamesPage, FriendsPage friendsPage, GroupsPage groupsPage, HistoryPage historyPage, SettingsPage settingsPage, TournamentsPage tournamentsPage)
        {
            _flyoutPage = flyoutPage;
            _quickMatchPage = quickMatchPage;
            _cashierPage = cashierPage;
            _customGamesPage = customGamesPage;
            _friendsPage = friendsPage;
            _groupsPage = groupsPage;
            _historyPage = historyPage;
            _settingsPage = settingsPage;
            _tournamentsPage = tournamentsPage;

            Flyout = _flyoutPage;
            Detail = new NavigationPage(_quickMatchPage);
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
                        Detail = new NavigationPage(_cashierPage);
                        break;
                    case { } cp when cp == typeof(CustomGamesPage):
                        Detail = new NavigationPage(_customGamesPage);
                        break;
                    case { } cp when cp == typeof(FriendsPage):
                        Detail = new NavigationPage(_friendsPage);
                        break;
                    case { } cp when cp == typeof(GroupsPage):
                        Detail = new NavigationPage(_groupsPage);
                        break;
                    case { } cp when cp == typeof(HistoryPage):
                        Detail = new NavigationPage(_historyPage);
                        break;
                    case { } cp when cp == typeof(QuickMatchPage):
                        Detail = new NavigationPage(_quickMatchPage);
                        break;
                    case { } cp when cp == typeof(SettingsPage):
                        Detail = new NavigationPage(_settingsPage);
                        break;
                    case { } cp when cp == typeof(TournamentsPage):
                        Detail = new NavigationPage(_tournamentsPage);
                        break;
                }

                IsPresented = false;
            }
        }
    }
}