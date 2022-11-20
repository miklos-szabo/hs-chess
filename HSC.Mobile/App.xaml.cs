using HSC.Mobile.Pages.ChessPage.ChessBoardPage;
using Keycloak.Net;
using Keycloak.Net.Models.ClientInitialAccess;
using Keycloak.Net.Models.Root;

namespace HSC.Mobile
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();

            MainPage = mainPage;
        }
    }
}