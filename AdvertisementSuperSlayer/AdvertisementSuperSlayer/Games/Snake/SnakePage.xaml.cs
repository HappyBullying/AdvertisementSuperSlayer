
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using AdvertisementSuperSlayer.Helpers;

namespace AdvertisementSuperSlayer.Games.Snake
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SnakePage : ContentPage
    {
        public SnakePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            
        }

        
    }
}