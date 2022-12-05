using HSC.Mobile.Services;
using System;

namespace HSC.Mobile.Pages.FriendsPage.ChatPage;

public partial class ChatPage : ContentPage
{
    public ChatViewModel ViewModel { get; set; }

    private readonly EventService _eventService;

    public ChatPage(ChatViewModel viewModel, EventService eventService)
	{
		ViewModel = viewModel;
        _eventService = eventService;
        BindingContext = viewModel;
		InitializeComponent();

        _eventService.ScrollToMove += ScrollTo;
	}

    private void ScrollTo(object sender, int index)
    {
        MessagesList.ScrollTo(ViewModel.Messages[index], ScrollToPosition.End, true);
    }
}