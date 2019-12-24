using System;
using System.Collections.Generic;
using AdvertisementSuperSlayer.DbModels;

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
            Puzzle, Snake, FindPair
        };

        public class GameResult
        {
            public GameResult(string Username, int Result, DateTime RecordDate)
            {
                this.Username = Username;
                this.Result = Result;
                this.RecordDate = RecordDate;
            }
            public string Username { get; set; }
            public int Result { get; set; }
            public DateTime RecordDate { get; set; }
        };

        public List<GameResult> Results = new List<GameResult>();

        public GameList()
        {
            InitializeComponent();
            Puzzle.Source = ImageSource.FromResource(App.PathToImages + "GameSelection.puzzle.png");
            Snake.Source = ImageSource.FromResource(App.PathToImages + "GameSelection.snake.png");
            FindPair.Source = ImageSource.FromResource(App.PathToImages + "GameSelection.findpair.png");

            GetDataFromTable(Filter.Puzzle);

            LeaderboardTable.ItemsSource = Results;
            this.BackgroundImageSource = ImageSource.FromResource(PathToImages + "leaderboard.png");
        }

        private async void GetDataFromTable(Filter filter)
        {
            switch (filter)
            {
                //insert filtering by game here
                case Filter.Puzzle:
                    {
                        List<PuzzleRecord> records = await App.Rest.GetPuzzleData();
                        foreach(PuzzleRecord rec in records)
                        {
                            Results.Add(new GameResult(rec.Username, (int)(rec.GameTime.TotalSeconds), rec.LastModified));
                        }
                        break;
                    }
                case Filter.Snake:
                    {
                        List<SnakeRecord> records = await App.Rest.GetSnakeData();
                        foreach(SnakeRecord rec in records)
                        {
                            Results.Add(new GameResult(rec.Username, rec.MaxScore, rec.LastModified));
                        }
                        break;
                    }
                case Filter.FindPair:
                    {
                        List<PairRecord> records = await App.Rest.GetPairData();
                        foreach (PairRecord rec in records)
                        {
                            Results.Add(new GameResult(rec.Username, (int)(rec.GameDuration.TotalSeconds), rec.LasModified));
                        }
                        break;
                    }
            }
            LeaderboardTable.ItemsSource = Results;
            

        }

        private void Puzzle_Clicked(object sender, EventArgs e)
        {
            Results.Clear();
            GetDataFromTable(Filter.Puzzle);
        }

        private void Snake_Clicked(object sender, EventArgs e)
        {
            Results.Clear();
            GetDataFromTable(Filter.Snake);
        }

        private void FindPair_Clicked(object sender, EventArgs e)
        {
            Results.Clear();
            GetDataFromTable(Filter.FindPair);
        }
    }
}