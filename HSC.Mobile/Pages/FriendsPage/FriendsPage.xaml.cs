using HSC.Mobile.Services;
using HSCApi;

namespace HSC.Mobile.Pages.FriendsPage;

public partial class FriendsPage : ContentPage
{
    public FriendsPage(FriendsViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}