using HSC.Mobile.Services;

namespace HSC.Mobile.Pages.HistoryPage.HistoryChessPage;

public partial class HistoryChessPage : ContentPage
{
    private readonly EventService _eventService;

	public HistoryChessPage(HistoryChessPageViewModel viewModel, EventService eventService)
	{
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