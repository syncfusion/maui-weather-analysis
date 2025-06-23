using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class ForecastModel
    {
        public DateTime Date { get; set; }

        public ImageSource? WeatherIcon { get; set; }

        public float DayTemperature { get; set; }

        public float NightTemperature { get; set; }

        public string? Weather { get; set; }

        public List<string>? WeatherCollection { get; set; }

        public float TempMinimum { get; set; }

        public float TempMaximum { get; set; }
        public float Temperature { get; set; }
    }
}
