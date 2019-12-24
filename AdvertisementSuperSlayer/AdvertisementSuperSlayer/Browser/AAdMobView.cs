using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Browser
{
    public class AAdMobView : View
    {
        public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
            nameof(AdUnitId),
            typeof(string),
            typeof(AAdMobView),
            string.Empty);

        public string AdUnitId
        {
            get => (string)GetValue(AdUnitIdProperty);
            set => SetValue(AdUnitIdProperty, value);
        }
    }
}
