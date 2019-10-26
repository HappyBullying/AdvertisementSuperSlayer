using AdvertisementSuperSlayer.Games.Pair.ViewModel;
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
        private PairViewModel bc;
        double WindowWidth { get; set; }
        double WindowHeight { get; set; }





        double tileSize;
        PhotoHalfPairTile[,] tiles = new PhotoHalfPairTile[NUM, NUM];
        public PairCards(int col, int row)
        {
            InitializeComponent();
            bc = new PairViewModel();
            BindingContext = bc;

            
            img1.Source = ImageSource.FromResource("AdvertisementSuperSlayer.Images.tank.png");
            img1.Scale = 0.1;

            AnimateBackground();


            Assembly assembly = GetType().GetTypeInfo().Assembly;
            SKBitmap TileBitmap;
            string[] backs = { "rot.png" };
            Stream stream = assembly.GetManifestResourceStream("AdvertisementSuperSlayer.Images." + backs[0]);
            TileBitmap = SKBitmap.Decode(stream);


            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    
                    PhotoHalfPairTile tile = new PhotoHalfPairTile();
                    tile.CoverBitmap = TileBitmap;
                    tile.Col = col;
                    tile.Row = row;
                    tiles[i, j] = tile;
                    tile.SetBinding(PhotoHalfPairTile.DegProperty, $"RotationDeg[{col}][{row}]", mode: BindingMode.TwoWay);
                    absoluteLayout.Children.Add(tile);
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
            tileSize = Math.Min(WindowWidth, WindowHeight) / NUM;
            absoluteLayout.WidthRequest = NUM * tileSize;
            absoluteLayout.HeightRequest = NUM * tileSize;

            foreach (View view in absoluteLayout.Children)
            {
                PhotoHalfPairTile tile = (PhotoHalfPairTile)view;
                tile.InvalidSurfaceState();
                // Set tile bounds.
                AbsoluteLayout.SetLayoutBounds(tile, new Rectangle(tile.Col * tileSize,
                                                                   tile.Row * tileSize,
                                                                   tileSize,
                                                                   tileSize));
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Task.Run(()=>{
                for (float i = 0; i < 180; i+= 3.0f)
                {
                    Task.Delay(17);
                    tiles[0, 2].Rotate(i);
                    tiles[3, 1].Rotate(i);
                }
            });
        }

        private void AnimateBackground()
        {
            //AnimateBackgroundLayer1();
            AnimateBackgroundLayer2();
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

        private async void AnimateBackgroundLayer2()
        {
            while (true)
            {
                await img1.TranslateTo((WindowWidth / 2.0) * 0.1, 0, 3000);
                
                //await img1.ScaleTo(0.8, 2750, Easing.SinOut);
                await img1.TranslateTo(-(WindowWidth / 2.0) * 0.1, 0, 3000);
                //await img1.ScaleTo(1, 2250, Easing.SinInOut);
                
            }
        }

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