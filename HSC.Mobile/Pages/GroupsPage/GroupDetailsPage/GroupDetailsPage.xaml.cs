namespace HSC.Mobile.Pages.GroupsPage.GroupDetailsPage;

public partial class GroupDetailsPage : ContentPage
{
    public GroupDetailsViewModel ViewModel { get; set; }

	public GroupDetailsPage(GroupDetailsViewModel viewModel)
    {
        ViewModel = viewModel;
        BindingContext = viewModel;
		InitializeComponent();
	}
}