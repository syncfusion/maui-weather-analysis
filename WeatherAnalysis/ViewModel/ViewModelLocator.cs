﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    internal class ViewModelLocator
    {
        private HistoricalWeatherViewModel? historicalWeatherViewModel;

        public HistoricalWeatherViewModel? HistoricalWeatherViewModel
        {
            get
            {
                if (historicalWeatherViewModel == null)
                    historicalWeatherViewModel = new HistoricalWeatherViewModel();
                return historicalWeatherViewModel;
            }

            set
            {
                historicalWeatherViewModel = value;
            }
        }

        private DayWeatherInfoViewModel? dayWeatherInfoViewModel;

        public DayWeatherInfoViewModel? DayWeatherInfoViewModel
        {
            get
            {
                if (dayWeatherInfoViewModel == null)
                    dayWeatherInfoViewModel = new DayWeatherInfoViewModel();
                return dayWeatherInfoViewModel;
            }

            set
            {
                dayWeatherInfoViewModel = value;
            }
        }

        private DailyForecastViewModel? forecastViewModel;

        public DailyForecastViewModel? DailyForecastViewModel
        {
            get
            {
                if (forecastViewModel == null)
                    forecastViewModel = new DailyForecastViewModel();
                return forecastViewModel;
            }

            set
            {
                forecastViewModel = value;
            }
        }

        private HourlyForecastViewModel? hourlyforecastViewModel;

        public HourlyForecastViewModel? HourlyForecastViewModel
        {
            get
            {
                if (hourlyforecastViewModel == null)
                    hourlyforecastViewModel = new HourlyForecastViewModel();
                return hourlyforecastViewModel;
            }

            set
            {
                hourlyforecastViewModel = value;
            }
        }

        private LocationViewModel? locationViewModel;

        public LocationViewModel? LocationViewModel
        {
            get
            {
                if (locationViewModel == null)
                    locationViewModel = new LocationViewModel();
                return locationViewModel;
            }

            set
            {
                locationViewModel = value;
            }
        }

        private TodayWeatherTileViewModel? todayWeatherTileViewModel;

        public TodayWeatherTileViewModel? TodayWeatherTileViewModel
        {
            get
            {
                if (todayWeatherTileViewModel == null)
                    todayWeatherTileViewModel = new TodayWeatherTileViewModel();
                return todayWeatherTileViewModel;
            }

            set
            {
                todayWeatherTileViewModel = value;
            }
        }

        private MapsViewModel? mapsViewModel;

        public MapsViewModel? MapsViewModel
        {
            get
            {
                if (mapsViewModel == null)
                    mapsViewModel = new MapsViewModel();
                return mapsViewModel;
            }

            set
            {
                mapsViewModel = value;
            }
        }
    }
}
