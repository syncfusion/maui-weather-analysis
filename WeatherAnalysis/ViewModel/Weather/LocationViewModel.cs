using Syncfusion.Maui.TabView;
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
    internal class LocationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LocationModel>? cityCollection;

        public ObservableCollection<LocationModel>? CityCollection
        {
            get { return cityCollection; }
            set
            {
                if (value != null)
                {
                    cityCollection = value;
                }
            }
        }

        private ObservableCollection<string>? temperatureFormatCollection;

        public ObservableCollection<string>? TemperatureFormatCollection
        {
            get { return temperatureFormatCollection; }
            set
            {
                if (value != null)
                    temperatureFormatCollection = value;
                RaisePropertyChanged(nameof(TemperatureFormatCollection));
            }
        }

        private LocationModel? selectedCity;

        public LocationModel? SelectedCity
        {
            get { return selectedCity; }
            set { selectedCity = value; RaisePropertyChanged(nameof(SelectedCity)); }
        }

        private string? selectedFormat;

        public string? SelectedFormat
        {
            get { return selectedFormat; }
            set { selectedFormat = value; RaisePropertyChanged(nameof(SelectedFormat)); }
        }  

        public LocationViewModel()
        {
            CityCollection = new ObservableCollection<LocationModel>();
            RefreshData(DateTime.Now);
            EventManager.Instance?.Subscribe<ForecastChangedEvent, ForecastChangedEventArgs>(OnForecastDateChanged);
            TemperatureFormatCollection = new ObservableCollection<string>
            {
                "°F",
                "°C"
            };
            ThemeName = lightTheme;
            SelectedCity = cityCollection?[0];
            SelectedFormat = temperatureFormatCollection?[0];
            SelectionChangedCommand?.Execute(selectedCity);
            FormatChanged?.Execute(selectedFormat);
        }

        private void RefreshData(DateTime dateTime)
        {
            CityCollection = DataStore.GetLocationDataForDate(dateTime);
        }
      
        private void OnForecastDateChanged(ForecastChangedEventArgs args)
        {
            if (args.Payload != null)
                RefreshData(args.Payload.Date);
        }

        private bool isCelsius = false;

        public bool IsCelsius
        {
            get { return isCelsius; }
            set { isCelsius = value; RaisePropertyChanged(nameof(IsCelsius)); }
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

        private ICommand? formatChanged;

        public ICommand? FormatChanged
        {
            get
            {
                if (formatChanged == null)
                {
                    formatChanged = new RelayCommand(FormatChangedExecute, CanFormatChanged);
                }

                return formatChanged;
            }
        }

        private ICommand? themeChanged;

        public ICommand? ThemeChanged
        {
            get
            {
                if (themeChanged == null)
                {
                    themeChanged = new RelayCommand(ThemeChangedExecute, CanThemeChanged);
                }

                return themeChanged;
            }
        }

        public void Dispose()
        {
            EventManager.Instance?.Unsubscribe<ForecastChangedEvent, ForecastChangedEventArgs>(OnForecastDateChanged);
            if (CityCollection != null)
            {
                CityCollection.Clear();
                CityCollection = null;
            }

            if (TemperatureFormatCollection != null)
            {
                TemperatureFormatCollection.Clear();
                TemperatureFormatCollection = null;
            }
        }

        private void ThemeChangedExecute(object parameter)
        {
            ThemeName = ThemeName == lightTheme ? darkTheme : lightTheme;
            UpdateTheme();
        }

        public void SelectionChangedExecute(object parameter)
        {
            EventManager.Instance?.Publish<CityChangedEvent, CityChangedEventArgs>(new CityChangedEventArgs()
            {
                Payload = SelectedCity?.City?.ToString()
            });
        }

        public void FormatChangedExecute(object parameter)
        {
            EventManager.Instance?.Publish<TempFormatChangedEvent, TempFormatChangedEventArgs>(new TempFormatChangedEventArgs()
            {
                Payload = SelectedFormat?.ToString()
            });

            IsCelsius = SelectedFormat?.ToString().Contains("C") ?? false;
        }

        public bool CanExecuteSelectionChanged(object parameter)
        {
            return true;
        }

        private bool CanFormatChanged(object parameter)
        {
            return true;
        }

        private bool CanThemeChanged(object parameter)
        {
            return true;
        }

        private string? themeName;

        public string? ThemeName
        {
            get
            {
                return themeName;
            }

            set
            {
                if (themeName != value)
                {
                    themeName = value;
                    UpdateTheme();
                }
            }
        }

        string lightTheme = "Windows11Light";
        string darkTheme = "Windows11Dark";

        void UpdateTheme()
        {
            //if (ThemeName == darkTheme)
            //{
            //    SfSkinManager.SetTheme(WindowHelper.MainWindow, new Theme() { ThemeName = darkTheme });
            //}
            //else
            //{
            //    SfSkinManager.SetTheme(WindowHelper.MainWindow, new Theme() { ThemeName = lightTheme });
            //}
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
