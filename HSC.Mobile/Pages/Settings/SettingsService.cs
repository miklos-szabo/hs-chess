using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Pages.Settings
{
    public sealed class SettingsService: ISettingsService
    {
        private const string LanguageKey = "language";
        private const string LanguageDefault = "en";

        private const string DarkThemeKey = "dark-theme";
        private const bool DarkThemeDefault = true;

        public string Language
        {
            get => Preferences.Get(LanguageKey, LanguageDefault);
            set => Preferences.Set(LanguageKey, value);
        }
        public bool DarkTheme
        {
            get => Preferences.Get(DarkThemeKey, DarkThemeDefault);
            set => Preferences.Set(DarkThemeKey, value);
        }
    }
}
