using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class DatetimeToStringConverter : IValueConverter
    {
        public string Format { get; set; } = "ddd dd"; // Default format

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                if (dateTime.Date == DateTime.Today.Date && Format != "dd" && Format != "ddd")
                {
                    return "Today";
                }

                return dateTime.ToString(Format, culture); // Ensure culture is applied
            }
            return "-";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class TimeSpanToStringConverter : IValueConverter
    {
        public string? Format { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime datetime)
            {
                return datetime.ToString(Format);
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
