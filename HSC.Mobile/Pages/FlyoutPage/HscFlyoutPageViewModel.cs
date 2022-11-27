﻿using System;
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
        private readonly NavigationService _navigationService;
        private UserMenuDto _userData;
        private readonly SignalrService _signalrService;

        public UserMenuDto UserData
        {
            get => _userData;
            set => SetField(ref _userData, value);
        }

        public HscFlyoutPageViewModel(AccountService accountService, IServiceProvider provider, AuthService authService, NavigationService navigationService, SignalrService signalrService)
        {
            _accountService = accountService;
            _provider = provider;
            _authService = authService;
            _navigationService = navigationService;
            _signalrService = signalrService;

            LogoutCommand = new Command(Logout);

            Task.Run(async () => await _signalrService.Connect());
            Task.Run(async () => await GetUserData());
        }

        public async Task GetUserData()
        {
            UserData = await _accountService.GetUserMenuDataAsync();
            _authService.UserName = UserData.UserName;
        }

        public void Logout()
        {
            _authService.Logout();
            _signalrService.Disconnect();
            _navigationService.ChangeMainPage(_provider.GetService<AuthenticationPage.AuthenticationPage>());
        }
    }
}
