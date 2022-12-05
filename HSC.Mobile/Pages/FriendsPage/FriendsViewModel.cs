using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using HSC.Mobile.Pages.QuickMatchPage.CreateCustomPage;
using HSC.Mobile.Resources.Translation;
using HSC.Mobile.Services;
using HSCApi;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.FriendsPage
{
    public class FriendsViewModel: BaseViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationService _navigationService;
        private readonly AlertService _alertService;
        private readonly FriendService _friendService;
        private readonly IStringLocalizer<Translation> _localizer;

        private FriendDto _selectedFriend;
        private ObservableCollection<FriendDto> _friends;
        private ObservableCollection<FriendRequestDto> _friendRequests;
        private string _addFriendText;

        public FriendDto SelectedFriend
        {
            get => _selectedFriend;
            set => SetField(ref _selectedFriend, value);
        }

        public ObservableCollection<FriendDto> Friends
        {
            get => _friends;
            set => SetField(ref _friends, value);
        }

        public ObservableCollection<FriendRequestDto> FriendRequests
        {
            get => _friendRequests;
            set => SetField(ref _friendRequests, value);
        }

        public string AddFriendText
        {
            get => _addFriendText;
            set => SetField(ref _addFriendText, value);
        }

        public ICommand AcceptFriendRequestCommand { get; set; }
        public ICommand DenyFriendRequestCommand { get; set; }
        public ICommand FriendClickedCommand { get; set; }
        public ICommand ChallengeFriendCommand { get; set; }
        public ICommand SendFriendRequestCommand { get; set; }

        public FriendsViewModel(IServiceProvider serviceProvider, NavigationService navigationService, AlertService alertService, FriendService friendService, IStringLocalizer<Translation> localizer)
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _alertService = alertService;
            _friendService = friendService;
            _localizer = localizer;

            AcceptFriendRequestCommand = new Command<int>(async id => await AcceptFriendRequest(id));
            DenyFriendRequestCommand = new Command<int>(async id => await DenyFriendRequest(id));
            FriendClickedCommand = new Command<string>(async name => await FriendClicked(name));
            ChallengeFriendCommand = new Command<string>(async name => await ChallengeFriend(name));
            SendFriendRequestCommand = new Command(async () => await SendFriendRequest());

            Task.Run(async () => await GetData());
        }

        public async Task GetData()
        {
            FriendRequests = (await _friendService.GetFriendRequestsAsync()).Where(r => r.IsIncoming).ToObservableCollection();
            Friends = (await _friendService.GetFriendsAsync()).ToObservableCollection();
        }

        public async Task AcceptFriendRequest(int requestId)
        {
           await _friendService.AcceptFriendRequestAsync(requestId);
           await GetData();
        }

        public async Task DenyFriendRequest(int requestId)
        {
            await _friendService.DeclineFriendRequestAsync(requestId);
            await GetData();
        }

        public async Task FriendClicked(string userName)
        {
            var chatPage = _serviceProvider.GetService<ChatPage.ChatPage>();
            chatPage.ViewModel.OtherUserName = userName;
            await _navigationService.PushAsync(chatPage);
        }

        public async Task ChallengeFriend(string userName)
        {
            var createCustomPage = _serviceProvider.GetService<CreateCustomPage>();
            createCustomPage.ViewModel.ToUserName = userName;
            await _navigationService.PushAsync(createCustomPage);
        }

        public async Task SendFriendRequest()
        {
            await _friendService.SendFriendRequestAsync(AddFriendText);
            await _alertService.DisplayInfoNoti(_localizer["Friends.RequestSent"]);
        }
    }
}
