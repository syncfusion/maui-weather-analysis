using WeatherAnalysis.Views.Desktop;

namespace WeatherAnalysis.View;



public partial class WeatherAnalysisViewDemo : ContentPage
{
	public WeatherAnalysisViewDemo()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        if (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            this.Content = new WeatherAnalysisView();
        else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            this.Content = new WeatherAndroidView();
    }
}