using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Resources.Translation;
using HSC.Mobile.Services;
using HSCApi;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.GroupsPage.GroupDetailsPage
{
    public class GroupDetailsViewModel: BaseViewModel
    {
        private readonly NavigationService _navigationService;
        private readonly GroupService _groupService;
        private readonly FriendService _friendService;
        private readonly AlertService _alertService;
        private readonly IStringLocalizer<Translation> _localizer;
        private readonly AuthService _authService;

        public GroupDetailsDto Details
        {
            get => _details;
            set => SetField(ref _details, value);
        }

        public string OwnUserName
        {
            get => _ownUserName;
            set => SetField(ref _ownUserName, value);
        }

        private int _groupId;
        private GroupDetailsDto _details;
        private string _ownUserName;

        public ICommand JoinCommand { get; set; }
        public ICommand SendMessageCommand { get; set; }
        public ICommand AddFriendCommand { get; set; }

        public GroupDetailsViewModel(NavigationService navigationService, GroupService groupService, FriendService friendService, AlertService alertService, IStringLocalizer<Translation> localizer, AuthService authService)
        {
            _navigationService = navigationService;
            _groupService = groupService;
            _friendService = friendService;
            _alertService = alertService;
            _localizer = localizer;
            _authService = authService;

            OwnUserName = _authService.UserName;

            JoinCommand = new Command(async () => await JoinGroup());
            SendMessageCommand = new Command<string>(async user => await SendMessageTo(user));
            AddFriendCommand = new Command<string>(async user => await AddFriend(user));
        }

        public int GroupId
        {
            get => _groupId; 
            set
            {
                SetField(ref _groupId, value);
                Task.Run(async () => Details = await _groupService.GetGroupDetailsAsync(GroupId));
            }
        }

        public async Task JoinGroup()
        {
            await _groupService.JoinGroupAsync(GroupId);
            Details = await _groupService.GetGroupDetailsAsync(GroupId);
            await _alertService.DisplaySuccessNoti(_localizer["Groups.Joined"]);
        }

        public async Task SendMessageTo(string username)
        {

        }

        public async Task AddFriend(string username)
        {
            await _friendService.SendFriendRequestAsync(username);
            await _alertService.DisplaySuccessNoti(_localizer["Friends.RequestSent"]);
        }
    }
}
