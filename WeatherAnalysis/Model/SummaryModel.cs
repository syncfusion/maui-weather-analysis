using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class SummaryModel
    {
        public DateTime Time { get; set; }
        public int Temperature { get; set; }
        public string Image { get; set; }
        public double YAxis { get; set; }
    }
}