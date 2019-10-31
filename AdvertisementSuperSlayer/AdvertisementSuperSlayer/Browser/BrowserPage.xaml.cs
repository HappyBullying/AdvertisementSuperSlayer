using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Browser
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowserPage : ContentPage
    {
        public BrowserPage()
        {
            InitializeComponent();
            WebView webV = new WebView();
            webV.Source = new UrlWebViewSource { Url = "http://localhost:5000" };
            webV.VerticalOptions = LayoutOptions.FillAndExpand;
            webV.HorizontalOptions = LayoutOptions.FillAndExpand;
            Content = webV;
            CountDown();
        }

        private async void CountDown()
        {
            await Task.Delay(6300);
            await Navigation.PopAsync();
        }
    }
}