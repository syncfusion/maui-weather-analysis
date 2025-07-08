using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class DayWeatherInfoViewModel : ObservableObject, INotifyPropertyChanged
    {
        private ObservableCollection<MoonPhaseModel> _moonPhases;
        public ObservableCollection<MoonPhaseModel> MoonPhases
        {
            get => _moonPhases;
            set
            {
                if (_moonPhases != value)
                {
                    _moonPhases = value;
                    OnPropertyChanged(nameof(MoonPhases));
                }
            }
        }

        public ObservableCollection<WeatherDataPoint> WeatherData { get; set; }
        public List<WeatherDataPoint> HighlightedData { get; set; }
        public ObservableCollection<DayWeatherInfoModel> Data { get; set; }

        public DayWeatherInfoViewModel()
        {
            //WeatherDataPoints
            WeatherData = new ObservableCollection<WeatherDataPoint>
            {
                    new WeatherDataPoint { Time = "6 AM", YAxis = 5 },
                    new WeatherDataPoint { Time = "8 AM", YAxis = 10 },
                    new WeatherDataPoint { Time = "10 AM", YAxis = 30 },
                    new WeatherDataPoint { Time = "12 PM", YAxis = 50 },
                    new WeatherDataPoint { Time = "2 PM", YAxis = 30 },
                    new WeatherDataPoint { Time = "4 PM", YAxis = 10 },
                    new WeatherDataPoint { Time = "6 PM", YAxis = 5 }
            };

            HighlightedData = WeatherData.Select(p => new WeatherDataPoint
            {
                Time = p.Time,
                YAxis = (p.YAxis >= 25 && p.YAxis <= 50) ? p.YAxis : (double?)null
            }).ToList();

            RefreshData();
            EventManager.Instance?.Subscribe<DataChangedEvent, EventArgs>(RefreshData);
            EventManager.Instance?.Subscribe<ForecastChangedEvent, ForecastChangedEventArgs>(OnForecastDateChanged);
            EventManager.Instance?.Subscribe<TempFormatChangedEvent, TempFormatChangedEventArgs>(FormatChanged);
        }

        private void FormatChanged(TempFormatChangedEventArgs args)
        {
            if (args.Payload != null)
                IsCelsius = args.Payload.Contains("C");
        }

        private void UpdateWeatherLife(DayWeatherInfoModel datamodel)
        {
            string weather = datamodel.Weather?.ToLowerInvariant() ?? string.Empty;

            // Umbrella Needed
            UmbrellaRequired = (weather.Contains("cloudy") || weather.Contains("rain") ||
                                weather.Contains("drizzle") || weather.Contains("thunderstorm"))
                                ? "Needed" : "No Need";

            // Outdoors
            Outdoors = (weather.Contains("cloudy") || weather.Contains("rain") ||
                        weather.Contains("thunderstorm") || weather.Contains("storm") ||
                        weather.Contains("snow"))
                        ? "Not Ideal" : "Great";

            // Clothing Suggestion
            ClothingSuggestion = datamodel.Temperature switch
            {
                >= 25 => "Shorts",
                >= 15 => "Light Jacket",
                _ => "Warm Clothes"
            };

            // UV Index State
            UVIndexState = datamodel.UVIndex switch
            {
                < 3 => "Low",
                < 6 => "Moderate",
                _ => "High"
            };

            // Wind Chill
            WindChill = datamodel.WindSpeed > 30 ? "Not Safe" : "Safe";

            // Air Quality
            AirQuality = datamodel.AirQuality switch
            {
                < 50 => "Good",
                < 100 => "Moderate",
                _ => "Unhealthy"
            };

            //Unit
            if (isCelsius)
                Unit = "C";
            else
                Unit = "F";
        }

        private void RefreshData(EventArgs? obj = null)
        {
            var datamodel = DataStore.GetSelectedTileWeather(selectedDateTime);
            City = datamodel.City;
            Weather = datamodel.Weather;
            Temperature = datamodel.Temperature;
            TemperatureDay = datamodel.TemperatureDay;
            TemperatureNight = datamodel.TemperatureNight;
            WindSpeed = datamodel.WindSpeed;
            Feelslike = datamodel.Feelslike;
            Humidity = datamodel.Humidity;
            SunRiseTime = datamodel.SunRiseTime;
            SunSetTime = datamodel.SunSetTime;
            MoonRiseTime = datamodel.MoonRiseTime;
            MoonSetTime = datamodel.MoonSetTime;
            UVIndex = datamodel.UVIndex;
            MaxTemperature = datamodel.MaxTemperature;
            MinTemperature = datamodel.MinTemperature;
            MoonPhase = datamodel.MoonPhase;
            Dew = datamodel.Dew;
            Precipitation = datamodel.Precipitation;
            MaxWind = datamodel.MaxWind;
            Interval = Math.Round((MaxTemperature - MinTemperature) / 2, 2);
            MinYValue = (int)MinTemperature - 1;
            MaxYValue = (int)MaxTemperature + 10;
            Cloudiness = datamodel.Cloudiness;
            TimeSpan duration = SunSetTime - SunRiseTime;
            TimeSpanForSunrise = $"{duration.Hours} hrs {duration.Minutes} mins";
            TimeSpan moonRiseDuration = moonSetTime - moonRiseTime;
            TimeSpanForMoonrise = $"{moonRiseDuration.Hours} hrs {moonRiseDuration.Minutes} mins";
            HourlyWeatherData = datamodel?.HourlyWeatherData != null ? new ObservableCollection<HourlyWeatherInfoModel>(datamodel.HourlyWeatherData) : new ObservableCollection<HourlyWeatherInfoModel>();
            WeatherCollection = datamodel?.WeatherCollection;
            UpdateMoonPhasesCollection(MoonPhase);
            UpdateWeatherLife(datamodel);
            MaximumYValueChart = double.NaN;
            MinimumYValueChart = double.NaN;
            ChartInterval = double.NaN;
            PrecipitationInMM = Math.Round(datamodel.Precipitation * 25.4, 2);
            CurrentTime = DateTime.Now.ToString("hh tt");
            labelYValue = Temperature + 3;
        }

        private void UpdateMoonPhasesCollection(string currentMoonPhase)
        {
            if (string.IsNullOrWhiteSpace(currentMoonPhase))
                return;

            var allMoonPhases = new List<MoonPhaseModel>
            {
                new MoonPhaseModel("New Moon", "ellipse.png"),
                new MoonPhaseModel("Waxing Crescent", "waxingcrescent.png"),
                new MoonPhaseModel("First Quarter", "firstquarter.png"),
                new MoonPhaseModel("Waxing Gibbous", "waxinggibbous.png"),
                new MoonPhaseModel("Full Moon", "fullmoon.png"),
                new MoonPhaseModel("Waning Gibbous", "waninggibbous.png"),
                new MoonPhaseModel("Last Quarter", "lastquarter.png"),
                new MoonPhaseModel("Waning Crescent", "waningcrescent.png")
            };

            var filteredPhases = allMoonPhases
                .Where(mp => mp.Name != currentMoonPhase)
                .ToList();

            if (MoonPhases == null)
                MoonPhases = new ObservableCollection<MoonPhaseModel>();

            MoonPhases.Clear();
            foreach (var phase in filteredPhases)
                MoonPhases.Add(phase);
        }


        public void Dispose()
        {
            EventManager.Instance?.Unsubscribe<DataChangedEvent, EventArgs>(RefreshData);
            EventManager.Instance?.Unsubscribe<ForecastChangedEvent, ForecastChangedEventArgs>(OnForecastDateChanged);
            EventManager.Instance?.Unsubscribe<TempFormatChangedEvent, TempFormatChangedEventArgs>(FormatChanged);
            if (HourlyWeatherData != null)
            {
                HourlyWeatherData.Clear();
                HourlyWeatherData = null;
            }
            if (WeatherCollection != null)
            {
                WeatherCollection.Clear();
                WeatherCollection = null;
            }
        }

        private string timeSpanForSunrise;

        public string TimeSpanForSunrise
        {
            get { return timeSpanForSunrise; }
            set { timeSpanForSunrise = value; RaisePropertyChanged(nameof(TimeSpanForSunrise)); }
        }

        private string timeSpanForMoonrise;

        public string TimeSpanForMoonrise
        {
            get { return timeSpanForMoonrise; }
            set { timeSpanForMoonrise = value; RaisePropertyChanged(nameof(TimeSpanForMoonrise)); }
        }

        private bool isCelsius = false;

        public bool IsCelsius
        {
            get { return isCelsius; }
            set { isCelsius = value; RaisePropertyChanged(nameof(IsCelsius)); }
        }

        private double interval;

        public double Interval
        {
            get { return interval; }
            set { interval = value; RaisePropertyChanged(nameof(Interval)); }
        }

        private int minYValue;

        public int MinYValue
        {
            get { return minYValue; }
            set { minYValue = value; RaisePropertyChanged(nameof(MinYValue)); }
        }

        private int maxYValue;

        public int MaxYValue
        {
            get { return maxYValue; }
            set { maxYValue = value; RaisePropertyChanged(nameof(MaxYValue)); }
        }

        private double chartInterval;

        public double ChartInterval 
        {
            get { return chartInterval; }
            set { chartInterval = value; RaisePropertyChanged(nameof(ChartInterval)); }
        }

        private double minimumYValueChart;

        public double MinimumYValueChart
        {
            get { return minimumYValueChart; }
            set { minimumYValueChart = value; RaisePropertyChanged(nameof(MinimumYValueChart)); }
        }

        private DateTime selectedDateTime = DateTime.Now;

        private void OnForecastDateChanged(ForecastChangedEventArgs obj)
        {
            if (obj.Payload == null) return;
            selectedDateTime = obj.Payload.Date;
            RefreshData(obj);
        }

        private string? city;

        public string? City
        {
            get { return city; }
            set
            {
                if (city != null)
                {
                    city = value;
                }
                RaisePropertyChanged(nameof(City));
            }
        }

        private float temperature;

        public float Temperature
        {
            get { return temperature; }
            set { temperature = value; RaisePropertyChanged(nameof(Temperature)); }
        }

        private float temperatureDay;

        public float TemperatureDay
        {
            get { return temperatureDay; }
            set { temperatureDay = value; RaisePropertyChanged(nameof(TemperatureDay)); }
        }

        private float temperatureNight;

        public float TemperatureNight
        {
            get { return temperatureNight; }
            set { temperatureNight = value; RaisePropertyChanged(nameof(TemperatureNight)); }
        }

        private float windSpeed;

        public float WindSpeed
        {
            get { return windSpeed; }
            set { windSpeed = value; RaisePropertyChanged(nameof(WindSpeed)); }
        }

        private float feelslike;

        public float Feelslike
        {
            get { return feelslike; }
            set { feelslike = value; RaisePropertyChanged(nameof(Feelslike)); }
        }

        private float humidity;

        public float Humidity
        {
            get { return humidity; }
            set { humidity = value; RaisePropertyChanged(nameof(Humidity)); }
        }

        private float rainChance;

        public float RainChance
        {
            get { return rainChance; }
            set { rainChance = value; RaisePropertyChanged(nameof(RainChance)); }
        }


        private string _currentTime;
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }



        private ImageSource? weatherIcon;

        public ImageSource? WeatherIcon
        {
            get { return weatherIcon; }
            set { weatherIcon = value; RaisePropertyChanged(nameof(WeatherIcon)); }
        }

        private DateTime sunRiseTime;

        public DateTime SunRiseTime
        {
            get { return sunRiseTime; }
            set { sunRiseTime = value; RaisePropertyChanged(nameof(SunRiseTime)); }
        }

        private DateTime sunSetTime;

        public DateTime SunSetTime
        {
            get { return sunSetTime; }
            set { sunSetTime = value; RaisePropertyChanged(nameof(SunSetTime)); }
        }

        private DateTime moonRiseTime;

        public DateTime MoonRiseTime
        {
            get { return moonRiseTime; }
            set { moonRiseTime = value; RaisePropertyChanged(nameof(MoonRiseTime)); }
        }

        private DateTime moonSetTime;

        public DateTime MoonSetTime
        {
            get { return moonSetTime; }
            set { moonSetTime = value; RaisePropertyChanged(nameof(MoonSetTime)); }
        }

        private float uvIndex;

        public float UVIndex
        {
            get { return uvIndex; }
            set { uvIndex = value; RaisePropertyChanged(nameof(UVIndex)); }
        }

        private float maxTemperature;
        private float minTemperature;

        public float MaxTemperature
        {
            get { return maxTemperature; }
            set { maxTemperature = value; RaisePropertyChanged(nameof(MaxTemperature)); }
        }

        public float MinTemperature
        {
            get { return minTemperature; }
            set { minTemperature = value; RaisePropertyChanged(nameof(MinTemperature)); }
        }

        private string? moonPhase;

        public string? MoonPhase
        {
            get { return moonPhase; }
            set
            {
                if (value != null)
                {
                    moonPhase = value;
                }
                RaisePropertyChanged(nameof(MoonPhase));
            }
        }

        public string formattedTemperature;
        public string FormattedTemperature
        {
            get
            {
                float convertedValue = isCelsius ? Temperature : (Temperature * 9 / 5) + 32;
                return $"{convertedValue:F2}° {(IsCelsius ? "C" : "F")}";
            }
            set { formattedTemperature = value; RaisePropertyChanged(nameof(FormattedTemperature)); }
        }

        private float dewPoint;

        public float Dew
        {
            get { return dewPoint; }
            set { dewPoint = value; RaisePropertyChanged(nameof(Dew)); }
        }

        private float precipitation;

        public float Precipitation
        {
            get { return precipitation; }
            set { precipitation = value; RaisePropertyChanged(nameof(Precipitation)); }
        }

        private float maxWind;

        public float MaxWind
        {
            get { return maxWind; }
            set { maxWind = value; RaisePropertyChanged(nameof(MaxWind)); }
        }

        private string? weather;

        public string? Weather
        {
            get { return weather; }
            set { weather = value; RaisePropertyChanged(nameof(Weather)); }
        }

        private string? umbrellaRequired;

        public string? UmbrellaRequired
        {
            get { return umbrellaRequired; }
            set { umbrellaRequired = value; RaisePropertyChanged(nameof(UmbrellaRequired)); }
        }

        private string? clothingSuggestion;

        public string? ClothingSuggestion
        {
            get { return clothingSuggestion; }
            set { clothingSuggestion = value; RaisePropertyChanged(nameof(ClothingSuggestion)); }
        }

        private string? outdoors;

        public string? Outdoors
        {
            get { return outdoors; }
            set { outdoors = value; RaisePropertyChanged(nameof(Outdoors)); }
        }

        public double PrecipitationInMM { get; set; }

        private string? uvIndexState;

        public string? UVIndexState
        {
            get { return uvIndexState; }
            set { uvIndexState = value; RaisePropertyChanged(nameof(UVIndexState)); }
        }

        private string? windChill;

        public string? WindChill
        {
            get { return windChill; }
            set { windChill = value; RaisePropertyChanged(nameof(WindChill)); }
        }

        private string? airQuality;

        public string? AirQuality
        {
            get { return airQuality; }
            set { airQuality = value; RaisePropertyChanged(nameof(AirQuality)); }
        }

        private string? unit = "F";

        public string? Unit
        {
            get { return unit; }
            set { unit = value; RaisePropertyChanged(nameof(Unit)); }
        }


        private float cloudiness;

        public float Cloudiness
        {
            get { return cloudiness; }
            set { cloudiness = value; RaisePropertyChanged(nameof(Cloudiness)); }
        }

        private float labelYValue;

        public float LabelYValue
        {
            get { return labelYValue; }
            set { labelYValue = value; RaisePropertyChanged(nameof(LabelYValue)); }
        }

        private ObservableCollection<HourlyWeatherInfoModel>? hourlyWeatherData;

        public ObservableCollection<HourlyWeatherInfoModel>? HourlyWeatherData
        {
            get { return hourlyWeatherData; }
            set { hourlyWeatherData = value; RaisePropertyChanged(nameof(HourlyWeatherData)); }
        }

        private List<string>? weatherCollection;

        public new event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<string>? WeatherCollection
        {
            get { return weatherCollection; }
            set { weatherCollection = value; RaisePropertyChanged(nameof(WeatherCollection)); }
        }

        public double MaximumYValueChart;     
        

    }
}
