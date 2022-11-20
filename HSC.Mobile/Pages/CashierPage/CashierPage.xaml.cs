namespace HSC.Mobile.Pages.CashierPage;

public partial class CashierPage : ContentPage
{
	public CashierPage(CashierViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}