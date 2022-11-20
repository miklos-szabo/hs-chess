namespace HSC.Mobile.Pages.Settings;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}