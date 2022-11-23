using HSC.Mobile.Helpers;
using HSC.Mobile.Pages.Settings;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSC.Mobile.Resources.Translation;

namespace HSC.Mobile.MarkupExtensions
{
    [ContentProperty(nameof(Key))]
    public class LocalizeExtension : IMarkupExtension
    {
        IStringLocalizer<Translation> _localizer;

        public string Key { get; set; } = string.Empty;

        public LocalizeExtension()
        {
            _localizer = ServiceHelper.GetService<IStringLocalizer<Translation>>();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return _localizer[Key];
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
    }
}
