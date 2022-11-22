using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Resources.Translation;
using HSC.Mobile.Services;
using Microsoft.Extensions.Localization;

namespace HSC.Mobile.Pages.AuthenticationPage
{
    public class AuthenticationViewModel: BaseViewModel
    {
        private readonly AuthService _authService;
        private readonly IServiceProvider _provider;
        private readonly IStringLocalizer<Translation> _localizer;
        private readonly AlertService _alertService;
        public ICommand LoginCommand { get; set; }

        private string _userName;
        private string _password;

        public string UserName
        {
            get => _userName;
            set => SetField(ref _userName, value);
        }

        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        public AuthenticationViewModel(AuthService authService, IServiceProvider provider, AlertService alertService, IStringLocalizer<Translation> localizer)
        {
            _authService = authService;
            _provider = provider;
            _alertService = alertService;
            _localizer = localizer;

            LoginCommand = new Command(async () => await Login());
        }

        public async Task Login()
        {
            if (await _authService.GetAndSaveToken(UserName, Password))
            {
                Application.Current.MainPage = _provider.GetService<MainPage>();
            }
            else
            {
                _alertService.ShowAlert(_localizer["Login.InvalidPopupTitle"], _localizer["Login.InvalidPopupDescription"], "OK");
            }
        }
    }
}
