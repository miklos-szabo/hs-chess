namespace HSC.Mobile.Pages.HistoryPage;

public partial class HistoryPage : ContentPage
{
	public HistoryPage(HistoryViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}