using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Pages.Settings
{
    public interface ISettingsService
    {
        string Language { get; set; }
        bool DarkTheme { get; set; }
    }
}
