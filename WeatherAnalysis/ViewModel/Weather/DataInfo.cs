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
    public class DataInfo 
    {
        private ObservableCollection<DataModel> bookInfo;

        public ObservableCollection<DataModel> BookInfo
        {
            get { return bookInfo; }
            set { this.bookInfo = value; }
        }

        public DataInfo()
        {
            GenerateBookInfo();
        }

        internal void GenerateBookInfo()
        {
            bookInfo = new ObservableCollection<DataModel>();
            bookInfo.Add(new DataModel() { Celsius = "90@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "40@", Condition = "summer", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "90@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "60@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "70@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "90@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "20@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "40@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "30@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "110@", Condition = "rain", Date = "Today" });
            bookInfo.Add(new DataModel() { Celsius = "93@", Condition = "rain", Date = "Today" });
        }
    }
}
