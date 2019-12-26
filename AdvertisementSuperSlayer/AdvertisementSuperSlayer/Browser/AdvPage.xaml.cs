using AdvertisementSuperSlayer.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace AdvertisementSuperSlayer.Browser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvPage : ContentPage
    {
        public AdvPage()
        {
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IOrientationService>().Portrait();
            }
            InitializeComponent();
            TapGestureRecognizer rec = new TapGestureRecognizer();
            rec.Tapped += RedirectToAd;
            AdvImg.GestureRecognizers.Add(rec);
            AdvImg.Source = ImageSource.FromResource(App.PathToImages + "Common.ad.ad.png");
        }

        [Obsolete]
        private async void RedirectToAd(object sender, EventArgs e)
        {
            AdvImg.Opacity = 0;
            Page adv = await Navigation.PopAsync();
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IOrientationService>().Landscape();
            }
            Device.OpenUri(new Uri("https://ads.google.com/"));
            await Navigation.PopAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            AdvImg.Opacity = 0;
            Page adv = await Navigation.PopAsync();
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IOrientationService>().Landscape();
            }
            await Navigation.PopAsync();
        }
    }
}