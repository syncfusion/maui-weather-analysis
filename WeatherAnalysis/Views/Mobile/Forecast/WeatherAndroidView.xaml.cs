using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Themes;
namespace WeatherAnalysis;


public partial class WeatherAndroidView : ContentView
{
    public WeatherAndroidView()
    {
        DataStore.Initialize(this.GetType().Assembly, BaseConfig.IsIndividualSB);
        InitializeComponent();
    }

    private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        if (e.NewValue.Text == "Hourly")
        {

            layoutPage.Content = new HourlyView();
        }
        else if (e.NewValue.Text == "Monthly")
        {
            layoutPage.Content = new Monthly();
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            var theme = mergedDictionaries.OfType<SyncfusionThemeResourceDictionary>().FirstOrDefault();
            if (theme != null)
            {
                if (theme.VisualTheme is SfVisuals.MaterialDark)
                {
                    theme.VisualTheme = SfVisuals.MaterialLight;
                    Application.Current.UserAppTheme = AppTheme.Light;
                }
                else
                {
                    theme.VisualTheme = SfVisuals.MaterialDark;
                    Application.Current.UserAppTheme = AppTheme.Dark;
                }
            }
        }
    }




    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var currentTheme = Application.Current.RequestedTheme;

        if (currentTheme == AppTheme.Dark)
        {
            darkthemebutton.IsChecked = true;

            if (themeLabel != null)
                themeLabel.Text = "Dark";
        }
        else
        {
            lightthemebutton.IsChecked = true;

            if (themeLabel != null)
                themeLabel.Text = "Light";
        }
        themeBottomSheet.Show();
    }

    private void comboBox_SelectionChanged(object sender, Syncfusion.Maui.Inputs.SelectionChangedEventArgs e)
    {
        if (e.AddedItems != null)
        {
            var item = e.AddedItems[0];
            var nameProperty = item.GetType().GetProperty("Name");

            if (nameProperty != null)
            {
                string? nameValue = nameProperty.GetValue(item)?.ToString();

                if (nameValue == "Humidity")
                {
                    summaryYAxis.Interval = double.NaN;
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
                    summaryYAxis.Maximum = double.NaN;
                    summaryYAxis.Minimum = double.NaN;
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



    private void SfRadioButton_StateChanging(object sender, Syncfusion.Maui.Buttons.StateChangingEventArgs e)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            var theme = mergedDictionaries.OfType<SyncfusionThemeResourceDictionary>().FirstOrDefault();
            if (theme != null)
            {
                theme.VisualTheme = SfVisuals.MaterialLight;
                Application.Current.UserAppTheme = AppTheme.Light;

                //Manually changing the Ellipse color, as setting it directly in the XAML page throws an error
                solarEllipse.Stroke = new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#FFFFFF"));
                solarEllipse.Fill = new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#E2227E"));
                lunarEllipse.Stroke = new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#FFFFFF"));
                lunarEllipse.Fill = new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#116DF9"));
            }
        }

        if (themeLabel != null)
        {
            themeLabel.Text = "Light";
        }
    }

    private void SfRadioButton_StateChanging_1(object sender, Syncfusion.Maui.Buttons.StateChangingEventArgs e)
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (mergedDictionaries != null)
        {
            var theme = mergedDictionaries.OfType<SyncfusionThemeResourceDictionary>().FirstOrDefault();
            if (theme != null)
            {
                theme.VisualTheme = SfVisuals.MaterialDark;
                Application.Current.UserAppTheme = AppTheme.Dark;

                //Manually changing the Ellipse color, as setting it directly in the XAML page throws an error
                solarEllipse.Stroke = new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#1C1B1D"));
                solarEllipse.Fill = new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#C9588E"));
                lunarEllipse.Stroke = new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#1C1B1D"));
                lunarEllipse.Fill = new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb("#BF3B49"));
            }
        }

        if (themeLabel != null)
        {
            themeLabel.Text = "Dark";
        }
    }

    private async void OnConfirmOpenLink(object sender, EventArgs e)
    {
        HelpSupportOverlay.IsVisible = false;
        var uri = new Uri("https://support.syncfusion.com/");
        await Launcher.Default.OpenAsync(uri);
    }

    private void OnCancelOpenLink(object sender, EventArgs e)
    {
        HelpSupportOverlay.IsVisible = false;
    }

    private void helpAndSupport_AnimationCompleted(object sender, EventArgs e)
    {
        HelpSupportOverlay.IsVisible = true;
    }

    private void themeGrid_AnimationCompleted(object sender, EventArgs e)
    {
        var currentTheme = Application.Current.RequestedTheme;

        if (currentTheme == AppTheme.Dark)
        {
            darkthemebutton.IsChecked = true;

            if (themeLabel != null)
                themeLabel.Text = "Dark";
        }
        else
        {
            lightthemebutton.IsChecked = true;

            if (themeLabel != null)
                themeLabel.Text = "Light";
        }
        themeBottomSheet.Show();
    }
}