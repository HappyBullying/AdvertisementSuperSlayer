using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Menue
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenue : ContentPage
    {
        public MainMenue()
        {
            InitializeComponent();
            BackgorundImage.Source = ImageSource.FromResource("AdvertisementSuperSlayer.Images.Menue.ekran-vybora.png");
            Snake.Source = ImageSource.FromResource("AdvertisementSuperSlayer.Images.Menue.knopka-1.png");
            Pair.Source = ImageSource.FromResource("AdvertisementSuperSlayer.Images.Menue.knopka-2.png");
            Pair.Aspect = Aspect.AspectFit;
            Snake.Aspect = Aspect.AspectFit;
        }
    }
}