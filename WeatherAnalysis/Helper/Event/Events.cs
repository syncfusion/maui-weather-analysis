﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    #region City Changed

    public delegate void CityChangedEvent(CityChangedEventArgs args);

    public class CityChangedEventArgs : SelectionChangedEventArgs<string>
    {
    }

    #endregion

    #region Data Changed

    public delegate void DataChangedEvent(EventArgs args);

    #endregion

    #region Temp Format Changed

    public delegate void TempFormatChangedEvent(TempFormatChangedEventArgs args);

    public class TempFormatChangedEventArgs : SelectionChangedEventArgs<string>
    {
    }

    #endregion

    #region Forecast Changed

    public delegate void ForecastChangedEvent(ForecastChangedEventArgs args);

    public class ForecastChangedEventArgs : SelectionChangedEventArgs<ForecastModel>
    {
    }
    #endregion

    #region Historic selection
    public delegate void HistoricChangedEvent(HistoricChangedEventArgs args);

    public class HistoricChangedEventArgs : SelectionChangedEventArgs<HistoricalWeatherInfoModel>
    {
    }
    #endregion
}
