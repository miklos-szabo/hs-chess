namespace HSC.Mobile.Pages.TournamentsPage.TournamentDetails;

public partial class TournamentDetailsPage : ContentPage
{
    public TournamentDetailsViewModel ViewModel { get; set; }
	public TournamentDetailsPage(TournamentDetailsViewModel viewModel)
	{
		ViewModel = viewModel;
        BindingContext = viewModel;
		InitializeComponent();
	}
}