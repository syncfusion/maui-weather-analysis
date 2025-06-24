using SampleBrowser.Maui.Base;
using WeatherAnalysis;
using static Microsoft.Maui.Controls.VisualStateManager;

namespace WeatherAnalysis;

public partial class WeatherView : ContentPage
{
    public WeatherView()
	{
        DataStore.Initialize(this.GetType().Assembly, BaseConfig.IsIndividualSB);
        InitializeComponent();

        Setter backgroundColorSetter = new() { Property = BackgroundColorProperty, Value = Colors.LightSkyBlue };
        VisualState stateSelected = new() { Name = CommonStates.Selected, Setters = { backgroundColorSetter } };
        VisualState stateNormal = new() { Name = CommonStates.Normal };
        VisualStateGroup visualStateGroup = new() { Name = nameof(CommonStates), States = { stateSelected, stateNormal } };
        VisualStateGroupList visualStateGroupList = new() { visualStateGroup };
        Setter vsgSetter = new() { Property = VisualStateGroupsProperty, Value = visualStateGroupList };
        Style style = new(typeof(Grid)) { Setters = { vsgSetter } };

        categoryList.SelectedItem = summaryViewModel.Categories[0];

        // Add the style to the resource dictionary
        Resources.Add(style);
        HorizontalScrollView1.Scrolled += OnScrollViewScrolled;
    }

    private void categoryCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection[0].ToString() == "Humidity") 
        {
            splineAreaSummary.ShowDataLabels = false;
            splineAreaSummary.YBindingPath = "Humidity";
            splineAreaSummary.Stroke = new SolidColorBrush(Colors.DarkBlue);

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0), // Starts from the top
                EndPoint = new Point(0.5, 1)   // Ends at the bottom
            };

            // Adjusting gradient stops to match the image
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue, Offset = 0.0F });  // Fully blue at the top
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue.WithAlpha(0.6f), Offset = 0.4F });  // Lighten towards the middle
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue.WithAlpha(0.3f), Offset = 0.7F });  // More transparent
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });  // Fully transparent at the bottom

            splineAreaSummary.Fill = gradientBrush;
        }

        else if(e.CurrentSelection[0].ToString() == "Wind") 
        {
            splineAreaSummary.ShowDataLabels = false;
            splineAreaSummary.YBindingPath = "Wind";
            splineAreaSummary.Stroke = new SolidColorBrush(Colors.DarkBlue);

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0), // Starts from the top
                EndPoint = new Point(0.5, 1)   // Ends at the bottom
            };

            // Adjusting gradient stops to match the image
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue, Offset = 0.0F });  // Fully blue at the top
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue.WithAlpha(0.6f), Offset = 0.4F });  // Lighten towards the middle
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.LightBlue.WithAlpha(0.3f), Offset = 0.7F });  // More transparent
            gradientBrush.GradientStops.Add(new GradientStop { Color = Colors.Transparent, Offset = 1.0F });  // Fully transparent at the bottom

            splineAreaSummary.Fill = gradientBrush;
        }

        else if(e.CurrentSelection[0].ToString() == "Temperature") 
        {
            //splineAreaSummary.Stroke = new SolidColorBrush(Colors.Transparent);
            //splineAreaSummary.YBindingPath = "Temperature";
            //splineAreaSummary.Fill = dayWeatherInfoViewModel.GradientBrush;
            //splineAreaSummary.ShowDataLabels = true;
        }
    }

    private void LocationComboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (e.AddedItems != null)
        {
            //locationView?.SelectionChangedExecute(e.AddedItems);
        }
    }

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


        }
    }

    private void OnScrollViewScrolled(object? sender, ScrolledEventArgs e)
    {
        forecastLeftArrow.IsVisible = HorizontalScrollView1.ScrollX > 0;
        forecastRightArrow.IsVisible = HorizontalScrollView1.ScrollX < HorizontalScrollView1.ContentSize.Width - HorizontalScrollView1.Width;
    }

    private async void forecastLeftArrow_Clicked(object sender, TappedEventArgs e)
    {
        await HorizontalScrollView1.ScrollToAsync(HorizontalScrollView1.ScrollX - 100, 0, true);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await HorizontalScrollView1.ScrollToAsync(HorizontalScrollView1.ScrollX + 100, 0, true);
    }
}