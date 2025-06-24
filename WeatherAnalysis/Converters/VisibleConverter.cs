using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class VisibleConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if ((bool.TryParse(value?.ToString(), out bool val)))
            {
                return val ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Visible;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
