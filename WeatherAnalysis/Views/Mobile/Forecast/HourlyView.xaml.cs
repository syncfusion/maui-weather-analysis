namespace WeatherAnalysis;

public partial class HourlyView : ContentView
{
	public HourlyView()
	{
		InitializeComponent();

        if(viewModel.HourlyWeatherData != null)
            dataGrid.HeightRequest = (double)viewModel.HourlyWeatherData.Count * dataGrid.RowHeight + dataGrid.HeaderRowHeight;
	}

    private void comboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (e.AddedItems != null)
        {
            var item = e.AddedItems[0];
            var nameProperty = item.GetType().GetProperty("Name");

            if (nameProperty != null)
            {
                if (nameProperty != null)
                {
                    string? nameValue = nameProperty.GetValue(item)?.ToString();

                    if (nameValue == "Humidity")
                    {
                        summaryYAxis.Maximum = double.NaN;
                        summaryYAxis.Interval = double.NaN;
                        summaryYAxis.Minimum = double.NaN;
                        splineAreaSummary.ShowDataLabels = false;
                        splineAreaSummary.YBindingPath = "Humidity";
                        splineAreaSummary.Stroke = new SolidColorBrush(Colors.DarkBlue);

                        var gradientBrush = new LinearGradientBrush
                        {
                            StartPoint = new Point(0.5, 0),
                            EndPoint = new Point(0.5, 1)
                        };

                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue, Offset = 0.0F });  // Fully blue at the top
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue.WithAlpha(0.6f), Offset = 0.4F });  // Lighten towards the middle
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue.WithAlpha(0.3f), Offset = 0.7F });  // More transparent
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });  // Fully transparent at the bottom

                        splineAreaSummary.Fill = gradientBrush;
                    }

                    else if (nameValue == "Wind")
                    {
                        summaryYAxis.Interval = double.NaN;
                        summaryYAxis.Minimum = double.NaN;
                        summaryYAxis.Maximum = double.NaN;
                        splineAreaSummary.ShowDataLabels = false;
                        splineAreaSummary.YBindingPath = "WindSpeed";
                        splineAreaSummary.Stroke = new SolidColorBrush(Colors.Purple);
                        var gradientBrush = new LinearGradientBrush
                        {
                            StartPoint = new Point(0.5, 0),
                            EndPoint = new Point(0.5, 1)
                        };

                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.MediumPurple, Offset = 0.0F });  // Fully blue at the top
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.MediumPurple.WithAlpha(0.6f), Offset = 0.4F });  // Lighten towards the middle
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.MediumPurple.WithAlpha(0.3f), Offset = 0.7F });  // More transparent
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });  // Fully transparent at the bottom

                        splineAreaSummary.Fill = gradientBrush;
                    }
                    else if (nameValue == "Temperature")
                    {
                        DayWeatherInfoViewModel dayWeatherInfoViewModel = new DayWeatherInfoViewModel();
                        summaryYAxis.Interval = double.NaN;
                        summaryYAxis.Minimum = double.NaN;
                        summaryYAxis.Maximum = dayWeatherInfoViewModel.MaxYValue;
                        splineAreaSummary.YBindingPath = "Temperature";
                        splineAreaSummary.Stroke = Colors.Transparent;
                        splineAreaSummary.ShowDataLabels = true;
                        splineAreaSummary.BindingContext = dayWeatherInfoViewModel;

                        var gradientBrush = new LinearGradientBrush
                        {
                            StartPoint = new Point(0.5, 0),
                            EndPoint = new Point(0.5, 1)
                        };

                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Orange, Offset = 0.0F });
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Yellow, Offset = 0.4F });
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightYellow, Offset = 0.7F });
                        gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });

                        splineAreaSummary.Fill = gradientBrush;


                        if (splineAreaSummary.LabelTemplate != null)
                            splineAreaSummary.LabelTemplate = new DataTemplate(() =>
                            {
                                var grid = new Grid();
                                var label = new Label
                                {
                                    HorizontalOptions = LayoutOptions.End,
                                    Margin = new Thickness(10, 0, 0, 0),
                                    FontSize = 12
                                };

                                label.SetBinding(Label.IsVisibleProperty, new Binding("Item.DateTime", BindingMode.OneWay, new LabelVisibilityConverter()));
                                label.SetBinding(Label.TextProperty, new Binding("Item.Temperature", BindingMode.OneWay, stringFormat: "{0:0}°"));

                                grid.Add(label);
                                return grid;
                            });
                    }
                }
            }
        }
    }

}