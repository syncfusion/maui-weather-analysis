using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TabItem> Tabs { get; set; }
        private TabItem selectedTab;
        private Microsoft.Maui.Controls.View currentView;

        public TabItem SelectedTab
        {
            get => selectedTab;
            set
            {
                if (selectedTab != value)
                {
                    selectedTab = value;
                    OnPropertyChanged();
                    LoadSelectedView();
                }
            }
        }

        public Microsoft.Maui.Controls.View CurrentView
        {
            get => currentView;
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            Tabs = new ObservableCollection<TabItem>
            {
                new TabItem { Name = "Today", ViewType = typeof(WeatherView) },
                new TabItem { Name = "Forecast", ViewType = typeof(HourlyView) },
                new TabItem { Name = "Trends", ViewType = typeof(WeatherView) },
                new TabItem { Name = "Radar", ViewType = typeof(WeatherView) },
                new TabItem { Name = "Settings", ViewType = typeof(WeatherView) }
            };

            SelectedTab = Tabs[0]; // Default selection
            LoadSelectedView();
        }

        private void LoadSelectedView()
        {
            if (SelectedTab?.ViewType != null)
            {
                var view = Activator.CreateInstance(SelectedTab.ViewType);
                if (view != null && view is Microsoft.Maui.Controls.View newView)
                {
                    CurrentView = newView;
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TabItem
    {
        public string Name { get; set; }
        public Type ViewType { get; set; }
    }
}