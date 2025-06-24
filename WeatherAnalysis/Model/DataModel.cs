using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class DataModel
    {
        private string date;
        private string condition;
        private string celsius;

        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public string Condition
        {
            get { return condition; }
            set
            {
                condition = value;
                OnPropertyChanged("Condition");
            }
        }

        public string Celsius
        {
            get { return celsius; }
            set
            {
                celsius = value;
                OnPropertyChanged("Celsius");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
