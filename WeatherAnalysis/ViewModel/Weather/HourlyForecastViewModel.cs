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
    public class HourlyForecastViewModel : INotifyPropertyChanged
    {
        private int hourlyforecastDays;

        public int HourlyForecastDays
        {
            get { return hourlyforecastDays; }
            set { hourlyforecastDays = value; }
        }

        private DateTime selectedDate;

        public DateTime SelectedDate
        {
            get
            {
                return selectedDate;
            }

            set
            {
                selectedDate = value;
                RaisePropertyChanged(nameof(SelectedDate));
            }
        }

        private ForecastModel? selectedTile;

        public ForecastModel? SelectedTile
        {
            get
            {
                return selectedTile;
            }

            set
            {
                if (value != null)
                {
                    selectedTile = value;
                }
                RaisePropertyChanged(nameof(SelectedTile));
            }
        }

        private ObservableCollection<ForecastModel>? _hourlyforecasts;

        public ObservableCollection<ForecastModel>? HourlyForecasts
        {
            get { return _hourlyforecasts; }
            set
            {
                if (value != null)
                {
                    _hourlyforecasts = value;
                }
                RaisePropertyChanged(nameof(HourlyForecasts));
            }
        }
        
        public HourlyForecastViewModel()
        {
            HourlyForecastDays = 14;
            HourlyForecasts = new ObservableCollection<ForecastModel>(DataStore.GetHourlyForecasts(hourlyforecastDays));
            EventManager.Instance?.Subscribe<DataChangedEvent, EventArgs>(OnDataChanged);
            if (SelectedTile == null)
            {
                SelectedTile = HourlyForecasts[0];
            }
        }

        private void OnDataChanged(EventArgs obj)
        {
            var currentDate = SelectedDate.Date;
            HourlyForecasts = new ObservableCollection<ForecastModel>(DataStore.GetHourlyForecasts(hourlyforecastDays));
            SelectedTile = HourlyForecasts.FirstOrDefault(f => f.Date.Date == currentDate);
        }

        public void Dispose()
        {
            EventManager.Instance?.Unsubscribe<DataChangedEvent, EventArgs>(OnDataChanged);
            if (HourlyForecasts != null)
            {
                HourlyForecasts.Clear();
                HourlyForecasts = null;
            }
        }

        private ICommand? selectionChangedCommand;

        public ICommand? SelectionChangedCommand
        {
            get
            {
                if (selectionChangedCommand == null)
                {
                    selectionChangedCommand = new RelayCommand(SelectionChangedExecute, CanExecuteSelectionChanged);
                }

                return selectionChangedCommand;
            }
        }

        private void SelectionChangedExecute(object parameter)
        {
            SelectedDate = SelectedTile != null ? SelectedTile.Date : DateTime.Now;
            EventManager.Instance?.Publish<ForecastChangedEvent, ForecastChangedEventArgs>(new ForecastChangedEventArgs() { Payload = SelectedTile });
        }

        private bool CanExecuteSelectionChanged(object parameter)
        {
            return true;
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
