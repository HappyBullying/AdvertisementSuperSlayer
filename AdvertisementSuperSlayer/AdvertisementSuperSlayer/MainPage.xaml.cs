using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly string PathToImages = App.PathToImages + "Menue.";
        public MainPage()
        {
            InitializeComponent();
            Play.Source = ImageSource.FromResource(PathToImages + "play.png");
            this.BackgroundImageSource = ImageSource.FromResource(PathToImages + "menu.png");
        }

        private async void OnPlayed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Games.GameSelectMenue());
        }
    }
}
