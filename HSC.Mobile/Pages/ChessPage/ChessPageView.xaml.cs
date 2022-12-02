using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using HSC.Mobile.Enums;
using HSC.Mobile.Pages.ChessPage;
using HSC.Mobile.Pages.ChessPage.ChessBoardPage;
using HSC.Mobile.Services;
using PonzianiComponents;

namespace HSC.Mobile
{
    public partial class ChessPageView : ContentPage
    {
        public ChessPageViewModel ViewModel { get; set; }
        private readonly EventService _eventService;

        public ChessPageView(ChessPageViewModel viewModel, EventService eventService)
        {
            ViewModel = viewModel;
            _eventService = eventService;
            BindingContext = viewModel;
            InitializeComponent();

            _eventService.ScrollToMove += ScrollToMove;
        }

        private void ScrollToMove(object sender, int index)
        {
            MovesListView.ScrollTo(index);
        }
    }
}