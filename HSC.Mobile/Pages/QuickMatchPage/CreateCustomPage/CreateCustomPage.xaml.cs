namespace HSC.Mobile.Pages.QuickMatchPage.CreateCustomPage;

public partial class CreateCustomPage : ContentPage
{
	public CreateCustomPage(CreateCustomViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}