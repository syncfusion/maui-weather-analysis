using Microsoft.Maui.Storage;
using Newtonsoft.Json;
using Syncfusion.Maui.Data;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
#if MACCATALYST
using Foundation;
#endif

namespace WeatherAnalysis
{
    public class DataStore
    {
        internal static Dictionary<string, List<DayDataDTO>>? Cache;
        internal static List<DayDataDTO>? DayData;
        internal static Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Tuple<string, string>>>>>? CityData;
        internal static Dictionary<string, HistoricalDataDTO[]>? CityHistoricData;
        internal static Dictionary<string, Dictionary<string, HashSet<string>>>? WeatherConditions;
        private static string? selectedCity;
        private static Dictionary<string, Tuple<string, string>>? CityProps;

        private static Assembly? Assembly;
        private static bool IsIndividualSB;


        private static void LoadWeatherData()
        {
            PrepareWeatherCondition();
            PrepareWeatherData();
            PrepareHistoricData();
            InitializeAllCityData();
        }

        public static void Initialize(Assembly assembly, bool isIndividualSB)
        {
            Assembly = assembly;
            IsIndividualSB = isIndividualSB;
            Cache = new Dictionary<string, List<DayDataDTO>>();
            DayData = new List<DayDataDTO>();
            CityData = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Tuple<string, string>>>>>();
            CityHistoricData = new Dictionary<string, HistoricalDataDTO[]>();
            WeatherConditions = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            EventManager.Instance?.Subscribe<CityChangedEvent, CityChangedEventArgs>(OnCityChanged);
            LoadWeatherData();
        }

        private static void InitializeAllCityData()
        {
            CityProps = new Dictionary<string, Tuple<string, string>>
            {
                { "New York", new Tuple<string, string>("74.0060 W", "40.7128 N") },
                //{"Los Angeles", new Tuple<string, string>("118.2437 W","34.0522 N")},
                //{"San Francisco",new Tuple<string, string>("122.4194 W","37.7749 N") },
                //{"Seattle",new Tuple<string, string>("122.3321 W","47.6062 N" )},
                //{"Washington",new Tuple<string, string>("77.0369 W","38.9072 N") }
            };

            foreach (var city in CityProps)
            {
                UpdateCurrentCity(city.Key);
            }
        }

        private static void OnCityChanged(CityChangedEventArgs obj)
        {
            UpdateCurrentCity(obj.Payload?.ToString()!);
            EventManager.Instance?.Publish<DataChangedEvent, EventArgs>(new EventArgs());
        }

        public static void UpdateCurrentCity(string city)
        {
            if (Cache != null && !Cache.ContainsKey(city))
            {
                var dayData = DataGenerator.GenerateWeatherData(city);

                Cache[city] = dayData;
            }

            if (selectedCity != city)
            {
                selectedCity = city;
            }

            DayData = Cache?[city];
        }

        public static void ReleaseData()
        {
            EventManager.Instance?.Unsubscribe<CityChangedEvent, CityChangedEventArgs>(OnCityChanged);
            if (Cache != null)
            {
                Cache.Clear();
                Cache = null;
            }

            if (DayData != null)
            {
                DayData.Clear();
                DayData = null;
            }

            if (CityHistoricData != null)
            {
                CityHistoricData.Clear();
                CityHistoricData = null;
            }

            if (CityData != null)
            {
                CityData.Clear();
                CityData = null;
            }

            if (WeatherConditions != null)
            {
                WeatherConditions.Clear();
                WeatherConditions = null;
            }

            if (CityProps != null)
            {
                CityProps.Clear();
                CityProps = null;
            }
        }

        private static void PrepareWeatherData()
        {
            var months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var weatherDetails = new string[] { "Temperature",  "Dew",
                "Feelslike",  "Snow",  "Humidity",
                "Precip",  "PrecipProb",  "Pressure",
                "UVIndex", "Visibility",  "WindSpeed",  "WindDirection",  "Sunrise",  "Sunset",  "Moonrise",  "Moonset",};

            var files = new string[] { "WeatherData-New York.json"};

            foreach (var file in files)
            {
                if (CityData != null)
                {
                    var city = Path.GetFileNameWithoutExtension(file).Replace("WeatherData-", "");
                    CityData.Remove(city);
                    var cityData = new Dictionary<string, Dictionary<string, Dictionary<string, Tuple<string, string>>>>();
                    CityData[city] = cityData;

                    try
                    {
                        var basePath = "WeatherAnalysis.Json." ;

                        using Stream? stream = Assembly.GetManifestResourceStream(basePath + file);
                        using StreamReader reader = new StreamReader(stream);
                        dynamic? parseJson = JsonConvert.DeserializeObject(reader.ReadToEnd());

                        foreach (var month in months)
                        {
                            var monthDetails = parseJson?[month];
                            if (monthDetails != null)
                            {
                                cityData.Remove(month);
                                var cityMonthData = new Dictionary<string, Dictionary<string, Tuple<string, string>>>();
                                cityData[month] = cityMonthData;
                                for (int i = 0; i < monthDetails.Count; i++)
                                {
                                    var condition = monthDetails[i].Condition.ToString();
                                    cityMonthData.Remove(condition.ToString());
                                    var cityMonthWeatherData = new Dictionary<string, Tuple<string, string>>();
                                    cityMonthData[condition] = cityMonthWeatherData;

                                    foreach (string detail in weatherDetails)
                                    {
                                        var min = monthDetails[i][$"{detail}_Min"].ToString();
                                        var max = monthDetails[i][$"{detail}_Max"].ToString();
                                        cityMonthWeatherData[detail] = new Tuple<string, string>(min, max);
                                    }
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }
        }

        public static void PrepareHistoricData()
        {
            CityHistoricData?.Clear();

            var files = new string[] { "HistoricData-New York.json" };
            foreach (var file in files)
            {
                var city = Path.GetFileNameWithoutExtension(file).Replace("HistoricData-", string.Empty);
                try
                {
                    var basePath = "WeatherAnalysis.Json.";

                    using Stream? stream = Assembly.GetManifestResourceStream(basePath + file);
                    using StreamReader reader = new StreamReader(stream);

                    using (StreamReader fileReader = new StreamReader(stream))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        HistoricalDataDTO[] weatherDataArray = (HistoricalDataDTO[])serializer.Deserialize(fileReader, typeof(HistoricalDataDTO[]))!;
                        if (CityHistoricData != null)
                            CityHistoricData[city] = weatherDataArray;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        public static void PrepareWeatherCondition()
        {
            WeatherConditions?.Clear();
            
            var months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            var files = new string[] {  "WeatherData-New York.json" };
            foreach (var file in files)
            {
                if (WeatherConditions != null)
                {
                    var city = Path.GetFileNameWithoutExtension(file).Replace("WeatherData-", string.Empty);
                    WeatherConditions.Remove(city);
                    var cityWeatherData = new Dictionary<string, HashSet<string>>();
                    WeatherConditions[city] = cityWeatherData;
                    try
                    {
                        var basePath = "WeatherAnalysis.Json.";

                        using Stream? stream = Assembly.GetManifestResourceStream(basePath + file);
                        using StreamReader reader = new StreamReader(stream);
                        dynamic? parseJson = JsonConvert.DeserializeObject(reader.ReadToEnd());

                        foreach (var month in months)
                        {
                            var monthDetails = parseJson?[month];
                            if (monthDetails != null)
                            {
                                cityWeatherData.Remove(month);
                                var cityMonthWeatherData = new HashSet<string>();

                                for (int i = 0; i < monthDetails.Count; i++)
                                {
                                    string conditions = monthDetails[i].Condition.ToString();
                                    var conditionsArray = conditions.Split(',').ToArray();
                                    foreach (var cond in conditionsArray)
                                    {
                                        cityMonthWeatherData.Add(cond.Trim());
                                    }
                                }

                                cityWeatherData[month] = cityMonthWeatherData;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                }
            }
        }

#if ANDROID
        public static dynamic? LoadJsonFromRawResource(string file)
        {
            var context = Android.App.Application.Context;
            var assetManager = context.Assets; 

            using (var stream = assetManager?.Open(file))
            using (var reader = new StreamReader(stream!))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject(json);
            }
        }
#endif

        #region Processing Data

        public static ForecastModel[] GetForecasts(int days)
        {
            var forecasts = new ForecastModel[days];

            // Loop through each item in the array
            var dayFromData = DayData?.FirstOrDefault(w => w.Datetime.Month == DateTime.Now.Month && w.Datetime.Day == DateTime.Now.Day);

            for (int i = 0; i < days; i++)
            {
                var dayToFetch = dayFromData?.Datetime.AddDays(i);
                var data = DayData?.FirstOrDefault(w => w.Datetime == dayToFetch);
                if (data == null)
                {
                    return forecasts;
                }

                forecasts[i] = new ForecastModel()
                {
                    Date = DateTime.Now.AddDays(i),
                    DayTemperature = data.DayTemp,
                    NightTemperature = data.NightTemp,
                    Weather = data.Conditions,
                    WeatherCollection = data?.HoulyDataCollection?.Select(we => we.Conditions)?.GroupBy(s => s)?.OrderByDescending(s => s.ToList().Count()).Select(s => s.Key).ToList()!,
                };
            }

            return forecasts;
        }

        public static ForecastModel[] GetHourlyForecasts(int days)
        {
            var forecasts = new ForecastModel[days];

            // Loop through each item in the array
            var dayFromData = DayData?.FirstOrDefault(w => w.Datetime.Month == DateTime.Now.Month && w.Datetime.Day == DateTime.Now.Day);

            for (int i = 0; i < days; i++)
            {
                var dayToFetch = dayFromData?.Datetime.AddDays(i);
                var data = DayData?.FirstOrDefault(w => w.Datetime == dayToFetch);
                if (data == null)
                {
                    return forecasts;
                }

                forecasts[i] = new ForecastModel()
                {
                    Weather = data.Conditions,
                    Date = DateTime.Now.AddDays(i),
                    Temperature = data.Temp,
                    WeatherCollection = data.HoulyDataCollection?.Select(we => we.Conditions).GroupBy(s => s).OrderByDescending(s => s.ToList().Count()).Select(s => s.Key).ToList()!,
                };
            }

            return forecasts;
        }

        public static ForecastModel[] GetMonthlyForecasts(int days, string? month)
        {
            var months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            int? monthNum = months.IndexOf(month) + 1;
            
            var forecasts = new ForecastModel[days];

            // Loop through each item in the array
            var dayFromData = DayData?.FirstOrDefault(w => w.Datetime.Month == monthNum && w.Datetime.Day == 1);

            for (int i = 0; i < days; i++)
            {
                var dayToFetch = dayFromData?.Datetime.AddDays(i);
                var data = DayData?.FirstOrDefault(w => w.Datetime == dayToFetch);
                if (data == null)
                {
                    return forecasts;
                }

                forecasts[i] = new ForecastModel()
                {
                    Weather = data.Conditions,
                    Date = data.Datetime,
                    TempMaximum = data.TempMax,
                    TempMinimum = data.TempMin,
                    WeatherCollection = data.HoulyDataCollection?.Select(we => we.Conditions).GroupBy(s => s).OrderByDescending(s => s.ToList().Count()).Select(s => s.Key).ToList()!,
                };
            }

            return forecasts;
        }

        internal static List<HistoricalWeatherInfoModel> GetHistoricalData()
        {
            if (CityHistoricData == null || CityHistoricData.Count == 0)
            {
                PrepareHistoricData();
            }

            var cityMonthHistoricData = CityHistoricData?[selectedCity!];
            List<HistoricalWeatherInfoModel>? historical = new List<HistoricalWeatherInfoModel>();
            if (cityMonthHistoricData != null)
            {
                foreach (var historicalData in cityMonthHistoricData)
                {
                    var monthData = new HistoricalWeatherInfoModel()
                    {
                        Month = historicalData.Month,
                        AvgLowTemperature = historicalData.AvgLowTemp,
                        AvgHighTemperature = historicalData.AvgHighTemp,
                        Rainfall = (int)Math.Ceiling(historicalData.RainFall),
                        SnowDays = historicalData.Snowfall,
                        Weather = historicalData.Weather,
                    };

                    historical.Add(monthData);
                }
            }
            return historical;
        }

        public static DayWeatherInfoModel GetSelectedTileWeather(DateTime selectedDate)
        {
            var dayFromData = DayData?.FirstOrDefault(w => w.Datetime.Month == selectedDate.Month && w.Datetime.Day == selectedDate.Day);
            if (dayFromData != null)
            {
                DayWeatherInfoModel currentWeather = new DayWeatherInfoModel();
                currentWeather.City = dayFromData.Name;
                currentWeather.Temperature = dayFromData.Temp;
                currentWeather.Weather = dayFromData.Conditions;
                currentWeather.UVIndex = dayFromData.UVindex!.Value;
                currentWeather.Precipitation = dayFromData.Precip;
                currentWeather.Cloudiness = dayFromData.Precipcover;
                currentWeather.SunRiseTime = dayFromData.Sunrise;
                currentWeather.SunSetTime = dayFromData.Sunset;
                currentWeather.MoonRiseTime = string.IsNullOrEmpty(dayFromData.Moonrise.ToString()) ? dayFromData.Sunset : DateTime.Parse(dayFromData.Moonrise.ToString());
                currentWeather.MoonSetTime = string.IsNullOrEmpty(dayFromData.Moonset.ToString()) ? dayFromData.Sunrise : DateTime.Parse(dayFromData.Moonset.ToString());
                currentWeather.MoonPhase = GetEnumDisplayName(dayFromData.MoonPhase);
                currentWeather.Dew = dayFromData.Dew;
                currentWeather.MaxTemperature = dayFromData.TempMax;
                currentWeather.MinTemperature = dayFromData.TempMin;
                ObservableCollection<HourlyWeatherInfoModel>? hourlyWeatherInfoModels = new ObservableCollection<HourlyWeatherInfoModel>();
                TimeSpan startOfDay = TimeSpan.Parse("06:00:00");
                TimeSpan endOfDay = TimeSpan.Parse("18:00:00");
                if (dayFromData.HoulyDataCollection != null)
                {
                    foreach (var hourDetails in dayFromData.HoulyDataCollection)
                    {
                        var hourlyData = new HourlyWeatherInfoModel();
                        hourlyData.Date = dayFromData.Datetime.ToLongDateString();
                        hourlyData.DateTime = hourDetails.Datetime;
                        hourlyData.Temperature = hourDetails.Temp;
                        hourlyData.Precipitation = hourDetails.Precip;
                        hourlyData.Weather = hourDetails.Conditions;
                        hourlyData.Humidity = hourDetails.Humidity;
                        hourlyData.UVIndex = hourDetails.UVindex;
                        hourlyData.DewPoint = hourDetails.Dew;
                        hourlyData.Pressure = hourDetails.Pressure;
                        hourlyData.Feelslike = hourDetails.Feelslike;
                        hourlyData.WindSpeed = hourDetails.Windspeed;
                        hourlyData.Visibility = hourDetails.Visibility;
                        hourlyWeatherInfoModels.Add(hourlyData);
                    }

                    currentWeather.HourlyWeatherData = hourlyWeatherInfoModels;
                }

                currentWeather.WeatherCollection = currentWeather.HourlyWeatherData?.Select(we => we.Weather).GroupBy(s => s).OrderByDescending(s => s.ToList().Count()).Select(s => s.Key).ToList()!;
                currentWeather.TemperatureDay = dayFromData.DayTemp;
                currentWeather.TemperatureNight = dayFromData.NightTemp;
                currentWeather.Feelslike = dayFromData.Feelslike;
                currentWeather.WindSpeed = dayFromData.Windspeed;
                currentWeather.Humidity = dayFromData.Humidity;
                return currentWeather;
            }
            return null!;
        }


        private static string? GetEnumDisplayName(Enum value)
        {
            var displayAttribute = value.GetType()?
                .GetField(value.ToString())?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            return displayAttribute != null ? displayAttribute.Name : value.ToString();
        }

        internal static ObservableCollection<LocationModel> GetLocationDataForDate(DateTime selectedDate)
        {
            ObservableCollection<LocationModel>? locationData = new ObservableCollection<LocationModel>();
            if (Cache != null)
            {
                foreach (var cityDate in Cache)
                {
                    var dayFromData = cityDate.Value.FirstOrDefault(w => w.Datetime.Month == selectedDate.Month && w.Datetime.Day == selectedDate.Day);
                    if (dayFromData != null)
                        locationData.Add(new LocationModel() { City = cityDate.Key, Longitude = CityProps?[cityDate.Key].Item1, Latitude = CityProps?[cityDate.Key].Item2, MinTemperature = dayFromData.TempMin, MaxTemperature = dayFromData.TempMax });
                }
            }
            return locationData;
        }

        #endregion
    }
}
