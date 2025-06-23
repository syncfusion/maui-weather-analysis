using Syncfusion.Maui.Charts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class SplineValueConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                //var ydata1 = (value as Spl);
                var ydata = (value as DayWeatherInfoViewModel).WeatherData;
                Brush interior;

                interior = ydata.First().YAxis < 30 ?
                new SolidColorBrush(Color.FromArgb("#E2227E")) :
                new SolidColorBrush(Color.FromArgb("#E7E0EC"));

                return interior;
            }
            return null;

        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
