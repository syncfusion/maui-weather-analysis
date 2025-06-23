using SampleBrowser.Maui.Base.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class StringToImageConverter : IValueConverter
    {
        static readonly Dictionary<string, string> WeatherLabelCollection = new Dictionary<string, string>()
        {
            { "Sunny_Rain", "light_rain_showers" },
            { "Sunny_Snow", "partly_sunny" },
            { "Sunny_Partiallycloudy", "mostly_sunny" },
            { "Sunny_Cloudy", "partly_sunny" },
            { "Sunny_FreezingRain", "mostly_sunny" },
            { "Rain_Snow", "wintry_mix" },
            { "Rain_Partiallycloudy", "light_rain" },
            { "Rain_Cloudy", "heavy_rain" },
            { "Rain_FreezingRain", "freezing_rain" },
            { "Rain_Sunny", "rain_showers" },
            { "Snow_Partiallycloudy", "snow_with_sun_and_cloudy" },
            { "Snow_Cloudy", "cloudy_with_snow" },
            { "Snow_FreezingRain", "wintry_mix" },
            { "Snow_Sunny", "snowy_with_sun" },
            { "Snow_Rain", "wintry_mix" },
            { "Partiallycloudy_Cloudy", "cloudy" },
            { "Partiallycloudy_FreezingRain", "rain_showers" },
            { "Partiallycloudy_Sunny", "mostly_cloudy" },
            { "Partiallycloudy_Rain", "light_rain_showers" },
            { "Partiallycloudy_Snow", "partial_snow" },
            { "Cloudy_FreezingRain", "light_rain_showers" },
            { "Cloudy_Sunny", "mostly_cloudy" },
            { "Cloudy_Rain", "Thunderstorm" },
            { "Cloudy_Snow", "cloudy_with_snow" },
            { "Cloudy_Partiallycloudy", "Cloudy" },
            { "FreezingRain_Sunny", "Mostly_rainy" },
            { "FreezingRain_Rain", "heavy_rain" },
            { "FreezingRain_Snow", "wintry_mix" },
            { "FreezingRain_Partiallycloudy", "light_freezing_rain_showers" },
            { "FreezingRain_Cloudy", "light_freezing_rain_showers" },
        };

       public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
       {
            string? _parameter = parameter?.ToString();

            if (value is IEnumerable<string> keys)
            {
                string drawingGroupName = string.Empty;
                List<string> processedKeys = keys.Select(key => key.Replace(" ", string.Empty)).ToList();

                foreach (string item in processedKeys.Take(2))
                {
                    if (!string.IsNullOrEmpty(drawingGroupName))
                    {
                        drawingGroupName += "_";
                    }
                    drawingGroupName += item;
                }

                if (_parameter == "Label")
                {
                    return WeatherLabelCollection.ContainsKey(drawingGroupName) ? WeatherLabelCollection[drawingGroupName] : "Unknown Weather";
                }

                if (WeatherLabelCollection.TryGetValue(drawingGroupName, out string? weatherLabel))
                {
#if IOS
                    return weatherLabel + ".png";
#else
                    return GetImageSource(weatherLabel);
#endif
                }
                return GetImageSource(drawingGroupName);
            }

            if (value is System.String weather)
            {
                weather = weather.Replace(" ", string.Empty);

                if (_parameter == "Moon")
                {
                    string resourcePath = $"SampleBrowser.Maui.Base.Resources.Images.{weather.ToLower()}.png";
                    return ImageSource.FromResource(resourcePath, typeof(SfImageResourceExtension).Assembly);
                }

                string defaultPath = $"SampleBrowser.Maui.Base.Resources.Images.{weather.ToLower()}.png";
                return ImageSource.FromResource(defaultPath, typeof(SfImageResourceExtension).Assembly);
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value;
        }

        private ImageSource GetImageSource(string resourceName)
        {
            return ImageSource.FromResource($"SampleBrowser.Maui.Base.Resources.Images.{resourceName}.png",typeof(SfImageResourceExtension).Assembly);
        }
    }
}
