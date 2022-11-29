using HSC.Mobile.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HSC.Mobile.Services;

namespace HSC.Mobile.Pages.Settings
{
    public class SettingsViewModel: BaseViewModel
    {
        private readonly SettingsService _settingsService;
        private readonly IServiceProvider _provider;
        private readonly NavigationService _navigationService;
        private bool _isHungarian;
        private bool _selectedDarkTheme;
        private int _selectedBoardTheme;

        public ICommand SaveCommand { get; set; }
        public ICommand ChangeBoardThemeCommand { get; set; }

        public bool IsHungarian
        {
            get => _isHungarian;
            set => SetField(ref _isHungarian, value);
        }

        public bool SelectedDarkTheme
        {
            get => _selectedDarkTheme;
            set => SetField(ref _selectedDarkTheme, value);
        }

        public int SelectedBoardTheme
        {
            get => _selectedBoardTheme;
            set => SetField(ref _selectedBoardTheme, value);
        }

        public SettingsViewModel(SettingsService settingsService, IServiceProvider provider, NavigationService navigationService)
        {
            _settingsService = settingsService;
            _provider = provider;
            _navigationService = navigationService;

            IsHungarian = _settingsService.Language == "hu";
            SelectedDarkTheme = _settingsService.DarkTheme;

            SaveCommand = new Command(Save);
            ChangeBoardThemeCommand = new Command(ChangeBoardTheme);
        }

        public void Save()
        {
            _settingsService.SetLanguage(IsHungarian ? "hu" : "en");

            _settingsService.BoardTheme = SelectedBoardTheme;

            if (_settingsService.DarkTheme != SelectedDarkTheme)
            {
                _settingsService.SetDarkTheme(SelectedDarkTheme);
            }

            _navigationService.ChangeMainPage(_provider.GetService<MainPage>());
        }

        public void ChangeBoardTheme()
        {
            SelectedBoardTheme = SelectedBoardTheme == 0 ? 1 : 0;
        }
    }
}
