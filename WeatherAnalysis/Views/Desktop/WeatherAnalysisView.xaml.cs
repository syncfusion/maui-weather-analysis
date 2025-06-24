using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Buttons;
using Syncfusion.Maui.Themes;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Layouts;


namespace WeatherAnalysis.Views.Desktop;

public partial class WeatherAnalysisView : ContentView
{
    private Microsoft.Maui.Controls.View _selectedBorder;
    private readonly List<Microsoft.Maui.Controls.View> _tabBorders = new();
    private string _selectedTheme;
    public WeatherAnalysisView()
	{
        DataStore.Initialize(this.GetType().Assembly, BaseConfig.IsIndividualSB);
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);

        AddTapGesture(TodayBorder);
        AddTapGesture(ForecastBorder);
        AddTapGesture(TrendsBorder);
        AddTapGesture(SettingsBorder);

        _tabBorders.Add(TodayBorder);
        _tabBorders.Add(ForecastBorder);
        _tabBorders.Add(TrendsBorder);
        _tabBorders.Add(SettingsBorder);

        _selectedBorder = TodayBorder;
        SetSelected(TodayBorder);
    }

    protected override void OnParentSet()
    {
        base.OnParentSet();

        if (Parent == null)
        {
            // This is like OnDisappearing
            DataStore.ReleaseData();
            Dispose(); // or custom cleanup logic
        }
    }

    public void Dispose()
    {

        // Clean up any view models or resources held by the ViewModelLocator
        if (this.BindingContext is LocationViewModel locationViewModel)
        {
            locationViewModel.Dispose();
        }

        ViewModelLocator locator = new()
        {
            DayWeatherInfoViewModel = null,
            HistoricalWeatherViewModel = null,
            DailyForecastViewModel = null,
            HourlyForecastViewModel = null,
            LocationViewModel = null,
            TodayWeatherTileViewModel = null,
            MapsViewModel = null
        };

        // Manually trigger garbage collection
        GC.Collect();
        GC.SuppressFinalize(this);
    }

    void AddTapGesture(Microsoft.Maui.Controls.View border)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => SetSelected(border);
        border.GestureRecognizers.Add(tapGesture);
    }

    void SetSelected(Microsoft.Maui.Controls.View border)
    {
        if (_selectedBorder is VerticalStackLayout prevLayout && prevLayout.Children.Count >= 2)
        {
            var prevIconLabel = prevLayout.Children[0] as Border;
            prevIconLabel.BackgroundColor = Colors.Transparent;
        }

        if (border is VerticalStackLayout layout && layout.Children.Count >= 2)
        {
            var iconLabel = layout.Children[0] as Border;
            var textLabel = layout.Children[1] as Label;
            var text = textLabel?.Text;

            if (Application.Current!.RequestedTheme == AppTheme.Dark)
            {
                iconLabel.BackgroundColor = Color.FromArgb("#35303C");
            }
            else
            {
                iconLabel.BackgroundColor = Color.FromArgb("#F7EDFF");
            }

            ContentView selectedContent = new();
            SettingsTab.IsVisible = false;
            selectedContent.IsVisible = true;
            switch (text)
            {
                case "Today":
                    var weatherPage = new WeatherView();
                    selectedContent.Content = weatherPage.Content;
                    break;
                case "Forecast":
                    ForecastPage forecast = new ForecastPage();
                    selectedContent.Content = forecast.Content;
                    break;
                case "Trends":
                    TrendsPage trends = new TrendsPage();
                    selectedContent.Content = trends.Content;
                    break;
                case "Settings":
                    SettingsTab.IsVisible = true;
                    selectedContent.IsVisible = false;
                    break;
            }

            selectedtab.Children.Clear();
            selectedtab.Children.Add(selectedContent);
        }

        _selectedBorder = border;
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {

        AppearancePopup.IsVisible = false;
        AppearanceOverlay.IsVisible = false;

        if (lightthemebutton.IsChecked == true)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                var theme = mergedDictionaries.OfType<SyncfusionThemeResourceDictionary>().FirstOrDefault();
                if (theme != null)
                {
                    theme.VisualTheme = SfVisuals.MaterialLight;
                    Application.Current.UserAppTheme = AppTheme.Light;
                }
            }
        }
        else if (darkthemebutton.IsChecked == true)
        {
            ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
            if (mergedDictionaries != null)
            {
                var theme = mergedDictionaries.OfType<SyncfusionThemeResourceDictionary>().FirstOrDefault();
                if (theme != null)
                {
                    theme.VisualTheme = SfVisuals.MaterialDark;
                    Application.Current.UserAppTheme = AppTheme.Dark;
                }
            }
        }

        UpdateTabColorsForTheme();
    }

    private void CloseIcon_Tapped(object sender, TappedEventArgs e)
    {
        AppearancePopup.IsVisible = false;
        AppearanceOverlay.IsVisible = false;
    }

    private void OnThemeTapped(object sender, EventArgs e)
    {
        var currentTheme = Application.Current.RequestedTheme;

        if (currentTheme == AppTheme.Dark)
        {
            darkthemebutton.IsChecked = true;
        }
        else
        {
            lightthemebutton.IsChecked = true;
        }

        AppearancePopup.IsVisible = true;
        AppearanceOverlay.IsVisible = true;

    }

    private void appearanceradiobutton_CheckedChanged(object sender, Syncfusion.Maui.Buttons.CheckedChangedEventArgs e)
    {
        _selectedTheme = lightthemebutton.Text;
        _selectedTheme = darkthemebutton.Text;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        AppearancePopup.IsVisible = false;
        AppearanceOverlay.IsVisible = false;
    }

    void UpdateTabColorsForTheme()
    {
        UpdateSingleTab(TodayBorder, isSelected: TodayBorder == _selectedBorder);
        UpdateSingleTab(ForecastBorder, isSelected: ForecastBorder == _selectedBorder);
        UpdateSingleTab(SettingsBorder, isSelected: SettingsBorder == _selectedBorder);
    }

    void UpdateSingleTab(VerticalStackLayout tab, bool isSelected)
    {
        if (tab.Children[0] is Border iconBorder && iconBorder.Content is Label iconLabel &&
            tab.Children[1] is Label textLabel)
        {
            if (isSelected)
            {
                iconBorder.BackgroundColor = Application.Current!.RequestedTheme == AppTheme.Light
                    ? Color.FromArgb("#F7EDFF")
                    : Color.FromArgb("#35303C");
            }
        }
    }

    private void HelpAndSupport_Tapped(object sender, TappedEventArgs e)
    {
        HelpSupportOverlay.IsVisible = true;
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
}