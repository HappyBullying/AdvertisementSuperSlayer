using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Play.Source = ImageSource.FromResource(App.PathToImages + "Menue.play.png");
            LeaderboardButton.Source = ImageSource.FromResource(App.PathToImages + "Leaderboard.leaderboardbutton.png");
            this.BackgroundImageSource = ImageSource.FromResource(App.PathToImages + "Menue.menu.png");
        }

        private async void OnPlayed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Games.GameSelectMenue());
        }

        private void OnLeaderboard(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Leaderboard.GameList());
        }
    }
}
