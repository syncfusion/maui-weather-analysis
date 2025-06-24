using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class WeatherDataPoint
    {
        public string Time { get; set; }

        public double? YAxis { get; set; }
        public Color StrokeColor => YAxis > 30 ? Colors.Red : Colors.Blue;
    }
}
