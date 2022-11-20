namespace HSC.Mobile.Pages.GroupsPage;

public partial class GroupsPage : ContentPage
{
	public GroupsPage(GroupsViewModel viewModel)
    {
        BindingContext = viewModel;
		InitializeComponent();
	}
}