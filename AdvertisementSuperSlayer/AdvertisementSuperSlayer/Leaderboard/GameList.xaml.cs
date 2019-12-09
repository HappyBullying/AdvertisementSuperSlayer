using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Leaderboard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameList : ContentPage
    {
        public static readonly string PathToImages = App.PathToImages + "Leaderboard.";
        public static readonly uint ResultsAmount = 10;
        public static readonly uint CellsAmount = 3;

        public enum Filter
        {
            All, Snake, FindPair
        };

        public class GameResult
        {
            public string Username;
            public int Result;
            public DateTime RecordDate;
        };

        public List<GameResult> Results = new List<GameResult>();

        public GameList()
        {
            InitializeComponent();
            All.Source = ImageSource.FromResource(PathToImages + "all.png");
            Snake.Source = ImageSource.FromResource(App.PathToImages + "GameSelection.snake.png");
            FindPair.Source = ImageSource.FromResource(App.PathToImages + "GemaSelection.findpair.png");


            ListView tableData = new ListView
            {
                // Source of data items.
                ItemsSource = Results,

                // Define template for displaying each item.
                // (Argument of DataTemplate constructor is called for 
                //      each item; it must return a Cell derivative.)
                ItemTemplate = new DataTemplate(() =>
                {
                    // Create views with bindings for displaying each property.
                    Label usernameLabel = new Label();
                    usernameLabel.SetBinding(Label.TextProperty, "Username");

                    Label resultLabel = new Label();
                    resultLabel.SetBinding(Label.TextProperty,
                        new Binding("Birthday", BindingMode.OneWay,
                            null, null, "Born {0:d}"));

                    BoxView boxView = new BoxView();
                    boxView.SetBinding(BoxView.ColorProperty, "FavoriteColor");

                    // Return an assembled ViewCell.
                    return new ViewCell
                    {
                        View = new Grid
                        {
                            
                            Children =
                                {
                                    boxView,
                                    new StackLayout
                                    {
                                        VerticalOptions = LayoutOptions.Center,
                                        Spacing = 0,
                                        Children =
                                        {
                                            usernameLabel,
                                            resultLabel
                                        }
                                        }
                                }
                        }
                    };
                })
            };

            for (int i = 1; i <= ResultsAmount; i++)
                for (int j = 0; j < CellsAmount; j++)
                {

                    //Headers.Children.Add(new BoxView {BackgroundColor = Color.Blue}, j, i);
                    //Headers.Children.Add(new Label {TextColor = Color.White, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center}, j, i);
                }
            GetDataFromTable(0);

            this.BackgroundImageSource = ImageSource.FromResource(PathToImages + "leaderboard.png");
        }

        private void GetDataFromTable(Filter filter)
        {
            switch (filter)
            {
                //insert filtering by game here
                case Filter.All:
                    break;
                case Filter.Snake:
                    break;
                case Filter.FindPair:
                    break;
            }

            for (int i = 1; i <= ResultsAmount; i++)
                for (int j = 0; j < CellsAmount; j+=2)
                {
                    int ind = i * (int)CellsAmount + j;
                    ((Label)Headers.Children.ElementAt(ind)).Text = "Test";
                }
                    
        }

        private void All_Clicked(object sender, EventArgs e)
        {
            GetDataFromTable(Filter.All);
        }

        private void Snake_Clicked(object sender, EventArgs e)
        {
            GetDataFromTable(Filter.Snake);
        }

        private void FindPair_Clicked(object sender, EventArgs e)
        {
            GetDataFromTable(Filter.FindPair);
        }
    }
}