using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class LocationModel
    {
        public string? City { get; set; }

        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        public float MinTemperature { get; set; }

        public float MaxTemperature { get; set; }
    }
}
