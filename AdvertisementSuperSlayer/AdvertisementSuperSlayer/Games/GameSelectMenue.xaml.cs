using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameSelectMenue : ContentPage
    {
        private readonly string PathToImages = App.PathToImages + "GameSelection.";
        public GameSelectMenue()
        {
            InitializeComponent();
            Snake.Source = ImageSource.FromResource(PathToImages + "snake.png");
            FindPair.Source = ImageSource.FromResource(PathToImages + "findpair.png");
            Puzzle.Source = ImageSource.FromResource(PathToImages + "puzzle.png");
            ComingSoon.Source = ImageSource.FromResource(PathToImages + "comingsoon.png");
            this.BackgroundImageSource = ImageSource.FromResource(PathToImages + "gameselection.png");
        }

        private async void Snake_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SnakeEater.SnakePageMain(20, 32));
        }

        private async void FindPair_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pair.PairCardsPage(4, 4));
        }

        private async void Puzzle_Clicked(object sender, EventArgs e)
        {

        }
    }
}