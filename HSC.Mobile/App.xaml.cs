namespace HSC.Mobile
{
    public partial class App : Application
    {
        public App(ChessPageView mainPage)
        {
            InitializeComponent();

            MainPage = mainPage;
        }
    }
}