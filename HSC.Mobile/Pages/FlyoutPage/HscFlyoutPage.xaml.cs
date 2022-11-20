namespace HSC.Mobile.Pages.FlyoutPage;

public partial class HscFlyoutPage : ContentPage
{
	public HscFlyoutPage(HscFlyoutPageViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}