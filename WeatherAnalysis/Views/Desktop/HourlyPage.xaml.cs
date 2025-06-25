
using Syncfusion.Maui.ListView;
using Syncfusion.Maui.Themes;
using System;

namespace WeatherAnalysis;

public partial class HourlyPage : ContentView
{
    public HourlyPage()
    {
        InitializeComponent();

        categoryList.SelectedItem = summaryViewModel.Categories[0];
        scrollView.Scrolled += OnScrollViewScrolled;
    }

    public int currentIndex = 0;
    private void SfListView_SelectionChanged(object sender, Syncfusion.Maui.ListView.ItemSelectionChangedEventArgs e)
    {
        if (e.AddedItems[0].ToString() == "Humidity")
        {
            summaryYAxis.Maximum = double.NaN;
            summaryYAxis.Minimum = double.NaN;
            summaryYAxis.Interval = double.NaN;
            splineAreaSummary.ShowDataLabels = false;
            splineAreaSummary.YBindingPath = "Humidity";
            splineAreaSummary.Stroke = new SolidColorBrush(Colors.DarkBlue);

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

            // Adjusting gradient stops to match the image
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue, Offset = 0.0F });  // Fully blue at the top
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue.WithAlpha(0.6f), Offset = 0.4F });  // Lighten towards the middle
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue.WithAlpha(0.3f), Offset = 0.7F });  // More transparent
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });  // Fully transparent at the bottom

            splineAreaSummary.Fill = gradientBrush;
        }
        else if (e.AddedItems[0].ToString() == "Precipitation")
        {
            summaryYAxis.Maximum = double.NaN;
            summaryYAxis.Minimum = double.NaN;
            summaryYAxis.Interval = double.NaN;
            splineAreaSummary.ShowDataLabels = false;
            splineAreaSummary.YBindingPath = "Precipitation";
            splineAreaSummary.Stroke = new SolidColorBrush(Colors.Red);

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

            // Adjusting gradient stops to match the image
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.OrangeRed, Offset = 0.0F });  // Fully blue at the top
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.DarkRed.WithAlpha(0.6f), Offset = 0.4F });  // Lighten towards the middle
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.DarkRed.WithAlpha(0.3f), Offset = 0.7F });  // More transparent
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });  // Fully transparent at the bottom

            splineAreaSummary.Fill = gradientBrush;
        }
        else if (e.AddedItems[0].ToString() == "UV")
        {
            summaryYAxis.Maximum = double.NaN;
            summaryYAxis.Minimum = double.NaN;
            summaryYAxis.Interval = double.NaN;
            splineAreaSummary.ShowDataLabels = false;
            splineAreaSummary.YBindingPath = "UVIndex";
            splineAreaSummary.Stroke = new SolidColorBrush(Colors.Green);

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightGreen, Offset = 0.0F });  // Fully blue at the top
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.DarkGreen.WithAlpha(0.6f), Offset = 0.4F });  // Lighten towards the middle
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Green.WithAlpha(0.3f), Offset = 0.7F });  // More transparent
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });  // Fully transparent at the bottom

            splineAreaSummary.Fill = gradientBrush;
        }
        else if (e.AddedItems[0].ToString() == "Wind")
        {
            summaryYAxis.Maximum = double.NaN;
            summaryYAxis.Minimum = double.NaN;
            summaryYAxis.Interval = double.NaN;
            splineAreaSummary.ShowDataLabels = false;
            splineAreaSummary.YBindingPath = "WindSpeed";
            splineAreaSummary.Stroke = new SolidColorBrush(Colors.Purple);
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1)
            };

            // Adjusting gradient stops to match the image
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.MediumPurple, Offset = 0.0F });  // Fully blue at the top
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.MediumPurple.WithAlpha(0.6f), Offset = 0.4F });  // Lighten towards the middle
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.MediumPurple.WithAlpha(0.3f), Offset = 0.7F });  // More transparent
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });  // Fully transparent at the bottom

            splineAreaSummary.Fill = gradientBrush;
        }
        else if (e.AddedItems[0].ToString() == "Temperature")
        {
            DayWeatherInfoViewModel dayWeatherInfoViewModel = new DayWeatherInfoViewModel();
            summaryYAxis.Maximum = dayWeatherInfoViewModel.MaxYValue;
            summaryYAxis.Minimum = double.NaN;
            summaryYAxis.Interval = double.NaN;
            summaryYAxis.ShowMinorGridLines = false;
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
                        Margin = new Thickness(20, 0, 0, 0),
                        FontSize = 10
                    };

                    label.SetBinding(Label.IsVisibleProperty, new Binding("Item.DateTime", BindingMode.OneWay, new LabelVisibilityConverter()));
                    label.SetBinding(Label.TextProperty, new Binding("Item.Temperature", BindingMode.OneWay, stringFormat: "{0:0}°"));

                    grid.Add(label);
                    return grid;
                });

            verticalLineAnnotation.Y2 = dayWeatherInfoViewModel.Temperature;
            textAnnotation.Y1 = dayWeatherInfoViewModel.Temperature + 3;
            textAnnotation.Text = "Now";
        }

        if (e.AddedItems[0].ToString() != "Temperature")
        {
            verticalLineAnnotation.Y2 = 0;
            textAnnotation.Text = string.Empty;
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await scrollView.ScrollToAsync(scrollView.ScrollX + 100, 0, true);
    }

    private async void forecastLeftArrow_Clicked(object sender, TappedEventArgs e)
    {
        await scrollView.ScrollToAsync(scrollView.ScrollX - 100, 0, true);
    }

    private void OnScrollViewScrolled(object? sender, ScrolledEventArgs e)
    {
        forecastLeftArrow.IsVisible = scrollView.ScrollX > 0;
        forecastRightArrow.IsVisible = scrollView.ScrollX < scrollView.ContentSize.Width - scrollView.Width;
    }
}