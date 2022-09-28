using HSC.Mobile.Resources.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Pages.Settings
{
    public class SettingsViewModel
    {
        public void ChangeTheme()
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                mergedDictionaries.Clear();
                mergedDictionaries.Add(new DarkTheme());
                mergedDictionaries.Add(new ComponentStyles());
            }
        }

        public void ChangeLanguage()
        {
            //thread.currentculture
        }
    }
}
