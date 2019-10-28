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
            webV.Source = new UrlWebViewSource { Url = "http://google.com" };
            webV.VerticalOptions = LayoutOptions.FillAndExpand;
            webV.HorizontalOptions = LayoutOptions.FillAndExpand;
            Content = webV;
        }
    }
}