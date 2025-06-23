using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class SummaryViewModel
    {
        public List<string> Categories { get; set; } = new() { "Temperature", "Precipitation", "UV", "Humidity", "Wind" };
        public ObservableCollection<SummaryModel> WeatherData { get; set; }
        public ObservableCollection<SummaryList> SummaryLists { get; set; }

        public SummaryViewModel()
        {
            WeatherData = new ObservableCollection<SummaryModel>
        {
            new SummaryModel { Time = new DateTime(2024, 2, 17, 9, 0, 0), Temperature = 22, Image = "mostly_cloudy.png" , YAxis=1},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 10, 0, 0), Temperature = 32, Image = "partly_sunny.png" , YAxis = 1.5},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 11, 0, 0), Temperature = 54, Image = "mostly_sunny.png" , YAxis = 2},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 12, 0, 0), Temperature = 27, Image = "sunny.png" , YAxis =2.5 },
            new SummaryModel { Time = new DateTime(2024, 2, 17, 13, 0, 0), Temperature = 41, Image = "sunny.png" , YAxis = 4},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 14, 0, 0), Temperature = 27, Image = "sunny.png" , YAxis = 5},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 15, 0, 0), Temperature = 27, Image = "sunny.png" , YAxis = 6},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 16, 0, 0), Temperature = 27, Image = "sunny.png"    , YAxis = 5},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 17, 0, 0), Temperature = 27, Image = "sunny.png" , YAxis = 4},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 18, 0, 0), Temperature = 27, Image = "sunny.png" , YAxis = 2.5},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 19, 0, 0), Temperature = 27, Image = "sunny.png" , YAxis = 2},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 20, 0, 0), Temperature = 27, Image = "sunny.png" , YAxis = 1.8},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 21, 0, 0), Temperature = 27, Image = "sunny.png" , YAxis = 1.5},
            new SummaryModel { Time = new DateTime(2024, 2, 17, 22, 0, 0), Temperature = 27, Image = "sunny.png", YAxis = 1}
        };
            SummaryLists = new ObservableCollection<SummaryList>
            {
                new SummaryList { Name = "Temperature"},
                new SummaryList { Name = "Humidity"},
                new SummaryList { Name = "Wind"},
            };
        }

        public class SummaryList
        {
            public string Name { get; set; }
        }
    }
}