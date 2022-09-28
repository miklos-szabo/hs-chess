using System.ComponentModel;
using System.Runtime.CompilerServices;
using HSC.Mobile.Enums;
using HSC.Mobile.Pages.ChessPage;
using HSC.Mobile.Pages.ChessPage.ChessBoardPage;
using PonzianiComponents;

namespace HSC.Mobile
{
    public partial class ChessPageView : ContentPage
    {
        public ChessPageView(ChessPageViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}