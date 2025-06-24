using SampleBrowser.Maui.Base;

namespace WeatherAnalysis;

public partial class Monthly : ContentView
{
	public Monthly()
	{
		DataStore.Initialize(this.GetType().Assembly, BaseConfig.IsIndividualSB);
		InitializeComponent();
	}

}