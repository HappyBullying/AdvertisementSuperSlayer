using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.Bitmap
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bmppage : ContentPage
    {
        static readonly SKBitmap originalBitmap =
            BitmapExtensions.LoadBitmapResource(typeof(Bmppage),
                "AdvertisementSuperSlayer.Images.extra_open.png");

        SKBitmap rotatedBitmap = originalBitmap;
        public Bmppage()
        {
            InitializeComponent();
        }
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            double radians = Math.PI * 60 / 180;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = originalBitmap.Width;
            int originalHeight = originalBitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);


            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            canvas.Translate(rotatedWidth / 2, rotatedHeight / 2);
            canvas.RotateDegrees(60f);
            canvas.Translate(-originalWidth / 2, -originalHeight / 2);
            canvas.DrawBitmap(rotatedBitmap, info.Rect, BitmapStretch.Uniform);
        }
    }
}