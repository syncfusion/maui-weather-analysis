using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeatherAnalysis
{
    public class WeatherDataPointViewModel : INotifyPropertyChanged
    {
        private string time;
        private int yAxis;

        // Model properties for a single data point
        public string Time
        {
            get => time;
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged(nameof(Time));
                }
            }
        }

        public int YAxis
        {
            get => yAxis;
            set
            {
                if (yAxis != value)
                {
                    yAxis = value;
                    OnPropertyChanged(nameof(YAxis));
                }
            }
        }

        // Static ObservableCollection to hold the data points
        public static ObservableCollection<WeatherDataPointViewModel> WeatherData { get; } = new ObservableCollection<WeatherDataPointViewModel>
    {
        new WeatherDataPointViewModel { Time = "6 AM", YAxis = 5 },
        new WeatherDataPointViewModel { Time = "8 AM", YAxis = 10 },
        new WeatherDataPointViewModel { Time = "10 AM", YAxis = 30 },
        new WeatherDataPointViewModel { Time = "12 PM", YAxis = 50 },
        new WeatherDataPointViewModel { Time = "2 PM", YAxis = 30 },
        new WeatherDataPointViewModel { Time = "4 PM", YAxis = 10 },
        new WeatherDataPointViewModel { Time = "6 PM", YAxis = 5 }
    };

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
