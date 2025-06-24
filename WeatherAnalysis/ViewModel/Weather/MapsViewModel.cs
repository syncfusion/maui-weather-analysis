using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class MapsViewModel : INotifyPropertyChanged
    {
        public MapsViewModel()
        {
            LocationDataCollection = new ObservableCollection<LocationModel>();
            RefreshData(DateTime.Now);
            EventManager.Instance?.Subscribe<ForecastChangedEvent, ForecastChangedEventArgs>(OnForecastDateChanged);
            EventManager.Instance?.Subscribe<TempFormatChangedEvent, TempFormatChangedEventArgs>(FormatChanged);
        }

        private void FormatChanged(TempFormatChangedEventArgs args)
        {
            if (args.Payload != null)
                IsCelsius = args.Payload.Contains("C");
        }

        private bool isCelsius = false;

        public bool IsCelsius
        {
            get { return isCelsius; }
            set { isCelsius = value; RaisePropertyChanged(nameof(IsCelsius)); }
        }
        private void RefreshData(DateTime dateTime)
        {
            LocationDataCollection = DataStore.GetLocationDataForDate(dateTime);
        }

        private void OnForecastDateChanged(ForecastChangedEventArgs args)
        {
            if (args.Payload != null)
                RefreshData(args.Payload.Date);
        }

        private ObservableCollection<LocationModel>? locationCollection;

        public ObservableCollection<LocationModel>? LocationDataCollection
        {
            get { return locationCollection; }
            set {
                if (value != null)
                {
                    locationCollection = value;
                }
                RaisePropertyChanged(nameof(LocationDataCollection)); }
        }

        public void Dispose()
        {
            EventManager.Instance?.Unsubscribe<ForecastChangedEvent, ForecastChangedEventArgs>(OnForecastDateChanged);
            EventManager.Instance?.Unsubscribe<TempFormatChangedEvent, TempFormatChangedEventArgs>(FormatChanged);
            if (LocationDataCollection != null)
            {
                LocationDataCollection.Clear();
                LocationDataCollection = null;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
