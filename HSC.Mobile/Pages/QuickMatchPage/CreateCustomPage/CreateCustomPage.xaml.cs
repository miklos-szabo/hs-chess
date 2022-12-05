namespace HSC.Mobile.Pages.QuickMatchPage.CreateCustomPage;

public partial class CreateCustomPage : ContentPage
{
    public CreateCustomViewModel ViewModel { get; set; }
	public CreateCustomPage(CreateCustomViewModel viewModel)
    {
        ViewModel = viewModel;
        BindingContext = viewModel;
		InitializeComponent();
	}
}