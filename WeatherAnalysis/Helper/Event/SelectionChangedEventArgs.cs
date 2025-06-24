using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class SelectionChangedEventArgs<T> : EventArgs
    {
        public T? Payload { get; set; }
    }
}
