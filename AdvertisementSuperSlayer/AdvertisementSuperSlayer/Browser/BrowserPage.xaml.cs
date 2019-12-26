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
        }

        public void Navigate(string source)
        {
            Device.StartTimer(TimeSpan.FromSeconds(12), CallBack);
            Browser.Source = source;
        }

        private bool CallBack()
        {
            NavigateBack();
            return false;
        }

        private async void NavigateBack()
        {
            await Navigation.PopAsync();
            await Navigation.PopAsync();
        }
    }
}