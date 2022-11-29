using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Pages.Settings;
using HSC.Mobile.Resources.Styles;

namespace HSC.Mobile.Services
{
    public sealed class SettingsService
    {
        private const string LanguageKey = "language";
        private const string LanguageDefault = "en";

        private const string DarkThemeKey = "dark-theme";
        private const bool DarkThemeDefault = true;

        private const string SquareLightKey = "square-light";
        private const string SquareLightDefault = "#f0d9b5";

        private const string SquareDarkKey = "square-dark";
        private const string SquareDarkDefault = "#b58863";

        private const string BoardThemeKey = "board-theme";
        private const int BoardThemeDefault = 0;

        public string Language
        {
            get => Preferences.Get(LanguageKey, LanguageDefault);
            private set => Preferences.Set(LanguageKey, value);
        }
        public bool DarkTheme
        {
            get => Preferences.Get(DarkThemeKey, DarkThemeDefault);
            private set => Preferences.Set(DarkThemeKey, value);
        }

        public int BoardTheme
        {
            get => Preferences.Get(BoardThemeKey, BoardThemeDefault);
            set => Preferences.Set(BoardThemeKey, value);
        }


        public void SetLanguage(string language)
        {
            Language = language;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(language);
            CultureInfo.CurrentCulture = new CultureInfo(language);
            CultureInfo.CurrentUICulture = new CultureInfo(language);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(language);
        }

        public void SetDarkTheme(bool dark)
        {
            DarkTheme = dark;
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(dark ? new DarkTheme() : new LightTheme());
                mergedDictionaries.Add(new ComponentStyles());
            }
        }

        public void StartupSetSettings()
        {
            SetLanguage(Language);
            SetDarkTheme(DarkTheme);
        }
    }
}
