using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class CelsiusToFahrenheitConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return null;

            if (values[0] == null || values[0] == BindableProperty.UnsetValue ||
                values[1] == null || values[1] == BindableProperty.UnsetValue)
            {
                return null;
            }

            if (!TryGetFloatValue(values[0], out float convertedValue))
                return null;

            bool isCelsius = values[1] is bool boolVal && boolVal;

            if (!isCelsius)
            {
                convertedValue = (convertedValue * 9 / 5) + 32;
            }

            if (values.Length == 3 && values[2] != null && values[2] != BindableProperty.UnsetValue)
            {
                if (!TryGetFloatValue(values[2], out float convertedSecondValue))
                    return null;

                if (!isCelsius)
                {
                    convertedSecondValue = (convertedSecondValue * 9 / 5) + 32;
                }

                return string.Format(parameter?.ToString() ?? "{0}, {1}", convertedValue, convertedSecondValue);
            }

            return string.Format(parameter?.ToString() ?? "{0}", convertedValue);
        }

        private bool TryGetFloatValue(object value, out float result)
        {
            if (value is float floatVal)
            {
                result = floatVal;
                return true;
            }

            return float.TryParse(value?.ToString(), out result);
        }

        public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
