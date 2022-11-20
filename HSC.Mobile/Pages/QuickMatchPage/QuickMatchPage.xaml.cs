namespace HSC.Mobile.Pages.QuickMatchPage;

public partial class QuickMatchPage : ContentPage
{
	public QuickMatchPage(QuickMatchViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}
}