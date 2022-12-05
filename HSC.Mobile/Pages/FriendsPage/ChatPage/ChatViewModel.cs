using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using HSC.Mobile.Services;
using HSCApi;
using ChatMessageDto = HSCApi.ChatMessageDto;

namespace HSC.Mobile.Pages.FriendsPage.ChatPage
{
    public class ChatViewModel: BaseViewModel
    {
        private readonly ChatService _chatService;
        private readonly SignalrService _signalrService;
        private readonly EventService _eventService;
        private AuthService _authService;

        private string _otherUserName;
        private ObservableCollection<ChatMessageDto> _messages = new();
        private string _ownUserName;

        public string TypedMessage { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public string OwnUserName
        {
            get => _ownUserName;
            set => SetField(ref _ownUserName, value);
        }

        public ChatViewModel(ChatService chatService, SignalrService signalrService, EventService eventService, AuthService authService)
        {
            _chatService = chatService;
            _signalrService = signalrService;
            _eventService = eventService;
            _authService = authService;

            OwnUserName = _authService.UserName;

            _signalrService.ChatMessageReceivedEvent += MessageReceived;

            SendMessageCommand = new Command(async () => await SendMessage());
        }

        private void MessageReceived(object sender, Services.ChatMessageDto e)
        {
            Messages.Add(new ChatMessageDto
            {
                Message = e.Message,
                SenderUserName = e.SenderUserName,
                TimeStamp = e.TimeStamp
            });
            _eventService.OnScrollToMove(Messages.Count - 1);
        }

        public string OtherUserName
        {
            get => _otherUserName;
            set
            {
                SetField(ref _otherUserName, value);
                Task.Run(async () => await GetChatMessages());
            }
        }

        public async Task GetChatMessages()
        {
            Messages = (await _chatService.GetChatMessagesAsync(OtherUserName, 30, 0)).Reverse().ToObservableCollection();
            _eventService.OnScrollToMove(Messages.Count - 1);
            await _chatService.MessagesReadAsync(OtherUserName);
        }

        public ObservableCollection<ChatMessageDto> Messages
        {
            get => _messages;
            set => SetField(ref _messages, value);
        }

        public async Task SendMessage()
        {
            await _chatService.SendChatMessageAsync(OtherUserName, TypedMessage);
            TypedMessage = string.Empty;
            await GetChatMessages();
        }
    }
}
