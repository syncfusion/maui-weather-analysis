﻿using System;
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
    public class DailyForecastViewModel : INotifyPropertyChanged
    {
        private int forecastDays;
        public int ForecastDays
        {
            get { return forecastDays; }
            set
            {
                forecastDays = value;
            }
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
                selectedTile = value;
                RaisePropertyChanged(nameof(SelectedTile));
            }
        }

        private ObservableCollection<ForecastModel>? _forecasts;

        public ObservableCollection<ForecastModel>? Forecasts
        {
            get { return _forecasts; }
            set { _forecasts = value; RaisePropertyChanged(nameof(Forecasts)); }
        }

        public DailyForecastViewModel()
        {
            ForecastDays = 10;
            Forecasts = new ObservableCollection<ForecastModel>(DataStore.GetForecasts(forecastDays));
            EventManager.Instance?.Subscribe<DataChangedEvent, EventArgs>(OnDataChanged);
            EventManager.Instance?.Subscribe<TempFormatChangedEvent, TempFormatChangedEventArgs>(OnFormatChanged);
            if (SelectedTile == null)
            {
                SelectedTile = Forecasts[0];
            }
        }

        private void OnFormatChanged(TempFormatChangedEventArgs args)
        {
            if (args.Payload != null)
                IsCelsius = args.Payload.Contains("C");
        }

        public void Dispose()
        {
            EventManager.Instance?.Unsubscribe<TempFormatChangedEvent, TempFormatChangedEventArgs>(OnFormatChanged);
            EventManager.Instance?.Unsubscribe<DataChangedEvent, EventArgs>(OnDataChanged);
            if (Forecasts != null)
            {
                Forecasts.Clear();
                Forecasts = null;
            }
        }

        private bool isCelsius = false;

        public bool IsCelsius
        {
            get { return isCelsius; }
            set { isCelsius = value; RaisePropertyChanged(nameof(IsCelsius)); }
        }

        private void OnDataChanged(EventArgs obj)
        {
            var currentDate = SelectedDate.Date;
            Forecasts = new ObservableCollection<ForecastModel>(DataStore.GetForecasts(forecastDays));
            SelectedTile = Forecasts.FirstOrDefault(f => f.Date.Date == currentDate);
        }

        private ICommand? selectionChangedCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
