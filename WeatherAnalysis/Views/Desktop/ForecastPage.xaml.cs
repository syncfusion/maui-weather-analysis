namespace WeatherAnalysis;
public partial class ForecastPage : ContentPage
{
	public ForecastPage()
	{
		InitializeComponent();
	}

    private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        if (e.NewValue.Text == "Hourly")
        {
            HourlyPage hourly = new HourlyPage();
            layoutPage.Content = hourly;
        }
        else if (e.NewValue.Text == "Monthly")
        {
            MonthlyPage monthly = new MonthlyPage();
            layoutPage.Content = monthly;
        }
    }
}