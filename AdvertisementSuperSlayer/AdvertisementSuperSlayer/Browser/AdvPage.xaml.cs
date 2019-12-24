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
    public partial class AdvPage : ContentPage
    {
        public AdvPage()
        {
            InitializeComponent();
            TapGestureRecognizer rec = new TapGestureRecognizer();
            rec.Tapped += RedirectToAd;
            AdvImg.GestureRecognizers.Add(rec);
            //AdvImg.Source = 
        }

        private async void RedirectToAd(object sender, EventArgs e)
        {

        }
    }
}