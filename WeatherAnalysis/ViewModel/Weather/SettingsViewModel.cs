using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class SettingsViewModel
    {
        public ObservableCollection<SettingsItem> SettingsItems { get; set; }

        public SettingsViewModel()
        {
            SettingsItems = new ObservableCollection<SettingsItem>
        {
            new SettingsItem { Icon = "&#xe793;", Title = "Units", Description = "Temperature, Wind" },
            new SettingsItem { Icon = "&#xe775;", Title = "Theme", Description = "System Default" },
            new SettingsItem { Icon = "&#xe75e;", Title = "Notification", Description = "Daily forecast, alerts" },
            new SettingsItem { Icon = "&#xe71f;", Title = "Help & Support", Description = "Syncfusion MAUI Controls" }
        };
        }
    }

    public class SettingsItem
    {
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
