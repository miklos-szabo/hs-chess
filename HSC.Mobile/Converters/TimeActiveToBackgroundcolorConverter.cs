using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Converters
{
    class TimeActiveToBackgroundcolorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Application.Current.Resources.TryGetValue("BackgroundColor2", out var activeColor);
            Application.Current.Resources.TryGetValue("BackgroundColor", out var passiveColor);
            return (bool)value ? activeColor : passiveColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
