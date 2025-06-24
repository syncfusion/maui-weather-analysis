using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class HistoricalWeatherInfoModel
    {
        public string? Month { get; set; }

        public int Rainfall { get; set; }

        public int SnowDays { get; set; }

        public float AvgLowTemperature { get; set; }

        public float AvgHighTemperature { get; set; }

        public string? Weather { get; set; }
    }
}
