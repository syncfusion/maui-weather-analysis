using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class ResponsiveUIConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not double windowDimension || parameter is not string dimensionType)
                return false;

            var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

            if (dimensionType == "Height")
            {
                const double MinHeight = 624;
                if (windowDimension < MinHeight)
                    return true;
                return false;
            }

            const double Ratio = 0.6;
            var minWidth = screenWidth * Ratio;

            if (windowDimension < minWidth)
                return true;

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class ColumnWidthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Visibility progressBarVisibility && progressBarVisibility == Visibility.Collapsed)
            {
                return new GridLength(0, GridUnitType.Auto);
            }

            return new GridLength(1, GridUnitType.Star);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
