using HSC.Mobile.Pages.AuthenticationPage;
using HSC.Mobile.Pages.ChessPage.ChessBoardPage;

namespace HSC.Mobile
{
    public partial class App : Application
    {
        public App(AuthenticationPage authPage)
        {
            InitializeComponent();

            MainPage = authPage;    // After login, MainPage is set to MainPage
        }
    }
}