namespace HSC.Mobile.Pages.AuthenticationPage;

public partial class AuthenticationPage : ContentPage
{
	public AuthenticationPage(AuthenticationViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}