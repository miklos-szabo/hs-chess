namespace HSC.Mobile.Pages.FriendsPage.ChatPage;

public partial class ChatPage : ContentPage
{
    public ChatViewModel ViewModel { get; set; }

	public ChatPage(ChatViewModel viewModel)
	{
		ViewModel = viewModel;
        BindingContext = viewModel;
		InitializeComponent();
	}
}