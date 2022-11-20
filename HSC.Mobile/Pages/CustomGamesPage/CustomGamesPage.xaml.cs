namespace HSC.Mobile.Pages.CustomGamesPage;

public partial class CustomGamesPage : ContentPage
{
	public CustomGamesPage(CustomGamesViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}