using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.Pair.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PairCards : ContentPage
    {
        List<string> FrontfaceNames = new List<string>
            {
                "1",
                "1",
                "2",
                "2",
                "3",
                "3",
                "4",
                "4",
                "5",
                "5",
                "6",
                "6",
                "7",
                "7",
                "8",
                "8" };
        public PairCards(int row, int col)
        {
            InitializeComponent();
            Rows = row;
            Cols = col;
            tiles = new PhotoHalfPairTile[row][];
            _Busy = new BusyBehavior();
            RightConfiguration += OnRightConfiguration;
            WrongConfiguration += OnWrongConfiguration;
            Init();
            //AnimateBackground();

            
        }


        private void Init()
        {
            BackG.Source = ImageSource.FromResource(PathToImages + "zirki-yarkie-na-fon.png");
            ImInfo = new SKImageInfo(150, 150);
            Random rnd = new Random(DateTime.Now.Millisecond);
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] BackTilePics = { "viol" };
            int backIndex = rnd.Next(BackTilePics.Length);
            Stream stream = assembly.GetManifestResourceStream(PathToImages + BackTilePics[backIndex] + ".png");
            TileBitmap = SKBitmap.Decode(stream).Resize(ImInfo, SKFilterQuality.High);
            stream.Close();
            stream = assembly.GetManifestResourceStream(PathToImages + "zel-galka.png");
            OkBitmap = SKBitmap.Decode(stream).Resize(ImInfo, SKFilterQuality.High);
            stream.Close();

            for (int i = 0; i < Rows; i++)
            {
                tiles[i] = new PhotoHalfPairTile[Cols];
                for (int j = 0; j < Cols; j++)
                {
                    int elem = rnd.Next(FrontfaceNames.Count);
                    string name = FrontfaceNames[elem];
                    FrontfaceNames.RemoveAt(elem);
                    stream = assembly.GetManifestResourceStream(PathToImages + "Front." + name + ".png");
                    SKBitmap front = SKBitmap.Decode(stream).Resize(ImInfo, SKFilterQuality.High);
                    tiles[i][j] = new PhotoHalfPairTile(TileBitmap, front);
                    tiles[i][j].Row = i;
                    tiles[i][j].Col = j;
                    tiles[i][j].FrontBitmapName = name;
                    TapGestureRecognizer tgr = new TapGestureRecognizer();
                    tgr.CommandParameter = tiles[i][j];
                    tgr.Command = new Command<PhotoHalfPairTile>(ExecuteRotation, CanExecuteRotation);
                    tiles[i][j].GestureRecognizers.Add(tgr);
                    absoluteLayout.Children.Add(tiles[i][j]);
                    stream.Close();
                }
            }
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
                PhotoHalfPairTile tile = (PhotoHalfPairTile)view;
                tile.InvalidSurfaceState();
                // Set tile bounds.
                AbsoluteLayout.SetLayoutBounds(tile, 
                    new Rectangle(tile.Col * tileSize, tile.Row * tileSize, tileSize, tileSize));
            }
        }



        private void AnimateBackground()
        {
            //AnimateBackgroundLayer1();
            //AnimateBackgroundLayer2();
            //AnimateBackgroundLayer3();
            //AnimateBackgroundLayer4();
        }

        //private async void AnimateBackgroundLayer1()
        //{
        //    while (true)
        //    {
        //        await BackgroundLayer1.ScaleTo(0.9, 2500, Easing.SinOut);
        //        await BackgroundLayer1.ScaleTo(1.2, 1750, Easing.SinInOut);
        //    }
        //}

        //private async void AnimateBackgroundLayer2()
        //{
        //    while (true)
        //    {
        //        await img1.TranslateTo((WindowWidth / 2.0) * 0.1, 0, 3000);

        //        //await img1.ScaleTo(0.8, 2750, Easing.SinOut);
        //        await img1.TranslateTo(-(WindowWidth / 2.0) * 0.1, 0, 3000);
        //        //await img1.ScaleTo(1, 2250, Easing.SinInOut);

        //    }
        //}

        //private async void AnimateBackgroundLayer3()
        //{
        //    while (true)
        //    {
        //        await BackgroundLayer3.ScaleTo(0.7, 3000, Easing.SinInOut);
        //        await BackgroundLayer3.ScaleTo(0.9, 2500, Easing.SinOut);
        //    }
        //}

        //private async void AnimateBackgroundLayer4()
        //{
        //    while (true)
        //    {
        //        await BackgroundLayer4.ScaleTo(0.6, 1750, Easing.SinOut);
        //        await BackgroundLayer4.ScaleTo(0.8, 2000, Easing.SinInOut);
        //    }
        //}
    }
}