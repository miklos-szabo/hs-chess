using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Services;
using HSCApi;

namespace HSC.Mobile.Pages.GroupsPage.CreateGroupPage
{
    public class CreateGroupViewModel: BaseViewModel
    {
        private readonly GroupService _groupService;
        private readonly NavigationService _navigationService;
        private string _title;
        private string _description;

        public string Title
        {
            get => _title;
            set => SetField(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        public ICommand CreateCommand { get; set; }

        public CreateGroupViewModel(GroupService groupService, NavigationService navigationService)
        {
            _groupService = groupService;
            _navigationService = navigationService;

            CreateCommand = new Command(async () => await Create());
        }

        public async Task Create()
        {
            await _groupService.CreateGroupAsync(Title, Description);
            await _navigationService.PopAsync();
        }
    }
}
