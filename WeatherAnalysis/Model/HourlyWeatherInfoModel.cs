using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class HourlyWeatherInfoModel : INotifyPropertyChanged
    {
        public string? Date { get; set; }

        public DateTime DateTime { get; set; }

        public float Temperature { get; set; }

        public bool IsCelsius { get; set; } 

        public string? Weather { get; set; }

        public float Precipitation { get; set; }

        public float Humidity { get; set; }

        public float? UVIndex { get; set; }

        public float DewPoint { get; set; }

        public float Pressure { get; set; }

        public float WindSpeed { get; set; }

        public float Feelslike { get; set; }

        public ImageSource? WeatherIcon { get; set; }

        public float Visibility { get; set; }

        public List<string>? WeatherCollection { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
