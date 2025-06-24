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
    public class HistoricalWeatherViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<HistoricalWeatherInfoModel>? historicalWeatherData;

        public ObservableCollection<HistoricalWeatherInfoModel>? HistoricalWeatherData
        {
            get { return historicalWeatherData; }
            set { historicalWeatherData = value; RaisePropertyChanged(nameof(HistoricalWeatherData)); }
        }

        private ObservableCollection<ForecastModel>? _monthlyWeatherData;

        public ObservableCollection<ForecastModel>? MonthlyWeatherData
        {
            get { return _monthlyWeatherData; }
            set
            {
                if (value != null)
                    _monthlyWeatherData = value;
                RaisePropertyChanged(nameof(MonthlyWeatherData));
            }
        }

        private HistoricalWeatherInfoModel? selectedTile;

        public HistoricalWeatherInfoModel? SelectedTile
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

        private int monthlyforecastDays;

        public int MonthlyForecastDays
        {
            get { return monthlyforecastDays; }
            set { monthlyforecastDays = value; }
        }

        public HistoricalWeatherViewModel()
        {
            var data = DataStore.GetHistoricalData();
            HistoricalWeatherData = new ObservableCollection<HistoricalWeatherInfoModel>(data);
            EventManager.Instance?.Subscribe<CityChangedEvent, CityChangedEventArgs>(OnCityChanged);
            EventManager.Instance?.Subscribe<HistoricChangedEvent,HistoricChangedEventArgs>(OnMonthChanged);
            if (SelectedTile == null)
            {
                SelectedTile = HistoricalWeatherData[0];
            }
            MonthlyForecastDays = GetDaysCount();
            MonthlyWeatherData = new ObservableCollection<ForecastModel>(DataStore.GetMonthlyForecasts(monthlyforecastDays, SelectedTile.Month));
        }

        private int GetDaysCount()
        {
            if (SelectedTile?.Month == "Feb")
                return 28;
            else if (SelectedTile?.Month == "Jan" || SelectedTile?.Month == "Mar" || SelectedTile?.Month == "May" || SelectedTile?.Month == "Jul" || SelectedTile?.Month == "Aug" || SelectedTile?.Month == " Oct" || SelectedTile?.Month == "Dec")
                return 31;
            else
                return 30;
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

        public void SelectionChangedExecute(object parameter)
        {
            //SelectedDate = SelectedTile != null ? SelectedTile.Date : DateTime.Now;
            EventManager.Instance?.Publish<HistoricChangedEvent, HistoricChangedEventArgs>(new HistoricChangedEventArgs() { Payload = SelectedTile });
        }

        private void OnMonthChanged(HistoricChangedEventArgs obj)
        {
            if (obj.Payload == null) return;
            SelectedTile = obj.Payload;
            MonthlyForecastDays = GetDaysCount();
            MonthlyWeatherData = new ObservableCollection<ForecastModel>(DataStore.GetMonthlyForecasts(monthlyforecastDays, SelectedTile.Month));
        }

        private bool CanExecuteSelectionChanged(object parameter)
        {
            return true;
        }

        private void OnCityChanged(CityChangedEventArgs args)
        {
            var data = DataStore.GetHistoricalData();
            HistoricalWeatherData = new ObservableCollection<HistoricalWeatherInfoModel>(data);
        }

        public void Dispose()
        {
            EventManager.Instance?.Unsubscribe<CityChangedEvent, CityChangedEventArgs>(OnCityChanged);
            EventManager.Instance?.Unsubscribe<HistoricChangedEvent, HistoricChangedEventArgs>(OnMonthChanged);
            if (HistoricalWeatherData != null)
            {
                HistoricalWeatherData.Clear();
                HistoricalWeatherData = null;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
