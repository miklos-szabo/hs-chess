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

namespace HSC.Mobile.Pages.GroupsPage
{
    public class GroupsViewModel: BaseViewModel
    {
        private readonly GroupService _groupService;
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationService _navigationService;
        private ObservableCollection<GroupDto> _ownGroups = new();
        private ObservableCollection<GroupDto> _otherGroups = new();
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetField(ref _searchText, value);
        }

        public ObservableCollection<GroupDto> OwnGroups
        {
            get => _ownGroups;
            set => SetField(ref _ownGroups, value);
        }

        public ObservableCollection<GroupDto> OtherGroups
        {
            get => _otherGroups;
            set => SetField(ref _otherGroups, value);
        }

        public ICommand SearchCommand { get; set; }
        public ICommand CreateGroupCommand { get; set; }
        public ICommand JoinCommand { get; set; }
        public ICommand DetailsCommand { get; set; }

        public GroupsViewModel(GroupService groupService, IServiceProvider serviceProvider, NavigationService navigationService)
        {
            _groupService = groupService;
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;

            SearchCommand = new Command(async () => await Search());
            CreateGroupCommand = new Command(async () => await CreateGroup());
            JoinCommand = new Command<int>(async id => await JoinGroup(id));
            DetailsCommand = new Command<int>(async id => await Details(id));

            Task.Run(async () => await Search());
        }

        public async Task Search()
        {
            var groups = await _groupService.GetGroupsAsync(SearchText);

            OwnGroups = groups.Where(g => g.IsInGroup).ToObservableCollection();
            OtherGroups = groups.Where(g => !g.IsInGroup).ToObservableCollection();
        }

        public async Task CreateGroup()
        {
            await _navigationService.PushAsync(new NavigationPage(_serviceProvider.GetService<CreateGroupPage.CreateGroupPage>()));
        }

        public async Task JoinGroup(int Id)
        {
            await _groupService.JoinGroupAsync(Id);
            await Search();
        }

        public async Task Details(int Id)
        {
            var detailsPage = _serviceProvider.GetService<GroupDetailsPage.GroupDetailsPage>();
            detailsPage.ViewModel.GroupId = Id;
            await _navigationService.PushAsync(new NavigationPage(detailsPage));
        }
    }
}
