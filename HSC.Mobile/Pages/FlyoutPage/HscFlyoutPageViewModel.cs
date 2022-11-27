using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Services;
using HSCApi;

namespace HSC.Mobile.Pages.FlyoutPage
{
    public class HscFlyoutPageViewModel: BaseViewModel
    {
        public ICommand LogoutCommand { get; set; }
        private readonly AccountService _accountService;
        private readonly IServiceProvider _provider;
        private readonly AuthService _authService;
        private UserMenuDto _userData;

        public UserMenuDto UserData
        {
            get => _userData;
            set => SetField(ref _userData, value);
        }

        public HscFlyoutPageViewModel(AccountService accountService, IServiceProvider provider, AuthService authService)
        {
            _accountService = accountService;
            _provider = provider;
            _authService = authService;

            LogoutCommand = new Command(Logout);

            Task.Run(async () => await GetUserData());
        }

        public async Task GetUserData()
        {
            UserData = await _accountService.GetUserMenuDataAsync();
        }

        public void Logout()
        {
            _authService.Logout();
            Application.Current.MainPage = _provider.GetService<AuthenticationPage.AuthenticationPage>();
        }
    }
}
