using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSC.Mobile.Converters
{
    public class DrawOfferToIconColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return "#ebb400";
                
            }
            else
            {
                Application.Current.Resources.TryGetValue("BackgroundColor", out var bgcolor);
                return bgcolor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
