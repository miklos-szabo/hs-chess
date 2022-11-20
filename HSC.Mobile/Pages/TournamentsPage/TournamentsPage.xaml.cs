namespace HSC.Mobile.Pages.TournamentsPage;

public partial class TournamentsPage : ContentPage
{
	public TournamentsPage(TournamentsViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}