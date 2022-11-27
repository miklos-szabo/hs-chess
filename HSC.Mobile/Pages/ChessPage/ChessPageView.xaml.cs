using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using HSC.Mobile.Enums;
using HSC.Mobile.Pages.ChessPage;
using HSC.Mobile.Pages.ChessPage.ChessBoardPage;
using PonzianiComponents;

namespace HSC.Mobile
{
    public partial class ChessPageView : ContentPage
    {
        public ChessPageViewModel ViewModel { get; set; }

        public ChessPageView(ChessPageViewModel viewModel)
        {
            ViewModel = viewModel;
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}