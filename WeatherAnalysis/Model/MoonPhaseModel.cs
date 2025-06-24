using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using SampleBrowser.Maui.Base.Converters;

namespace WeatherAnalysis
{
    public class MoonPhaseModel : ObservableObject
    {
        public string Name { get; set; }

        public string ImageFileName { get; set; }

        public ImageSource ImageSource =>
            ImageSource.FromResource(
                $"SampleBrowser.Maui.Base.Resources.Images.{ImageFileName}",
                typeof(SfImageResourceExtension).Assembly);

        private int _imageSize;
        public int ImageSize
        {
            get => _imageSize;
            set => SetProperty(ref _imageSize, value);
        }

        public MoonPhaseModel(string name, string imageFileName)
        {
            Name = name;
            ImageFileName = imageFileName;
            ImageSize = 30; // Default size
        }
    }

}
