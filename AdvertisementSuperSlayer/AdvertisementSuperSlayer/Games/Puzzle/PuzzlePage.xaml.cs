using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.Puzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PuzzlePage : ContentPage
    {
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        
        private double tileSize;
        private readonly string pathToImages = "AdvertisementSuperSlayer.Images.";
        private int Rows;
        private int Cols;
        private PhotoPuzzleElement[][] PuzzleElements;
        public PuzzlePage(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Rows = 5;
            Cols = 8;
            InitializeComponent();
            Init();
            HandleImage(pathToImages + "Pair.pair_background.png");
            System.Collections.Generic.set
        }


        private void Init()
        {
            PuzzleElements = new PhotoPuzzleElement[Rows][];
            for (int i = 0; i < Rows; i++)
                PuzzleElements[i] = new PhotoPuzzleElement[Cols];
        }

        void OnContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            WindowWidth = contentView.Width;
            WindowHeight = contentView.Height;

            if (WindowWidth <= 0 || WindowHeight <= 0)
                return;

            // Orient StackLayout based on portrait/landscape mode.
            stackLayout.Orientation = (WindowWidth < WindowHeight) ? StackOrientation.Vertical :
                                                         StackOrientation.Horizontal;

            // Calculate tile size and position based on ContentView size.
            tileSize = Math.Min(WindowWidth, WindowHeight) / Math.Min(Rows, Cols);
            absoluteLayout.WidthRequest = Cols * tileSize;
            absoluteLayout.HeightRequest = Rows * tileSize;

            foreach (View view in absoluteLayout.Children)
            {
                PhotoPuzzleElement tile = (PhotoPuzzleElement)view;
                tile.InvalidSurfaceState();
                // Set tile bounds.
                AbsoluteLayout.SetLayoutBounds(tile,
                    new Rectangle(tile.Col * tileSize, tile.Row * tileSize, tileSize, tileSize));
            }
        }
    }
}