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

namespace AdvertisementSuperSlayer.Games.Pair
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PairCards : ContentPage
    {

        double tileSize;
        public static int NUM = 4;
        PhotoHalfPairTile[,] tiles = new PhotoHalfPairTile[NUM, NUM];
        public PairCards()
        {
            InitializeComponent();
            img1.Source = ImageSource.FromResource("AdvertisementSuperSlayer.Images.tank.png");
            mg.HeightRequest = 1000;
            mg.WidthRequest = 1880;
            
            mg.Source = ImageSource.FromResource("AdvertisementSuperSlayer.Images.6sImDJmIBsw.jpg");
            AnimateBackground();
            for (int i = 0; i < NUM; i++)
            {
                for (int j = 0; j < NUM; j++)
                {
                    ImageSource imageSource = ImageSource.FromResource("AdvertisementSuperSlayer.Images.rot.png");

                    Assembly assembly = GetType().GetTypeInfo().Assembly;
                    SKBitmap TileBitmap;
                    using (Stream stream = assembly.GetManifestResourceStream("AdvertisementSuperSlayer.Images.rot.png"))
                    {
                        TileBitmap = SKBitmap.Decode(stream);
                    }

                    PhotoHalfPairTile tile = new PhotoHalfPairTile(i, j, "AdvertisementSuperSlayer.Images.rot.png", TileBitmap);
                    tile.InvalidSurfaceState();
                    tiles[i, j] = tile;
                    absoluteLayout.Children.Add(tile);
                }
            }
        }
        void OnContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            double width = contentView.Width;
            double height = contentView.Height;

            if (width <= 0 || height <= 0)
                return;

            // Orient StackLayout based on portrait/landscape mode.
            stackLayout.Orientation = (width < height) ? StackOrientation.Vertical :
                                                         StackOrientation.Horizontal;

            // Calculate tile size and position based on ContentView size.
            tileSize = Math.Min(width, height) / NUM;
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
                await img1.ScaleTo(0.8, 2750, Easing.SinOut);
                //await BackgroundLayer2.TranslateTo(1900, 300);
                await img1.ScaleTo(1, 2250, Easing.SinInOut);
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