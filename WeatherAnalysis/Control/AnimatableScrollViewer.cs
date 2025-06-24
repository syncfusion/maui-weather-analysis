using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAnalysis
{
    public class AnimatableScrollView : ScrollView
    {
        public static readonly BindableProperty AnimatableHorizontalOffsetProperty =
            BindableProperty.Create(nameof(AnimatableHorizontalOffset), typeof(double), typeof(AnimatableScrollView), 0.0,
                propertyChanged: OnHorizontalOffsetChanged);

        public double AnimatableHorizontalOffset
        {
            get => (double)GetValue(AnimatableHorizontalOffsetProperty);
            set => SetValue(AnimatableHorizontalOffsetProperty, value);
        }

        private static void OnHorizontalOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AnimatableScrollView scrollView)
            {
                scrollView.ScrollToAsync((double)newValue, 0, true); // Smooth scroll animation
            }
        }
    }
}
