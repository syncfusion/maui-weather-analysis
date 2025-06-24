using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public enum MoonPhase
    {
        [Display(Name = "New Moon")]
        NewMoon,
        [Display(Name = "First Quarter")]
        FirstQuarter,
        [Display(Name = "Full Moon")]
        FullMoon,
        [Display(Name = "Last Quarter")]
        LastQuarter,
        [Display(Name = "Waxing Crescent")]
        WaxingCrescent,
        [Display(Name = "Waxing Gibbous")]
        WaxingGibbous,
        [Display(Name = "Waning Gibbous")]
        WaningGibbous,
        [Display(Name = "Waning Crescent")]
        WaningCrescent
    }
}
