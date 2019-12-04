using SkiaSharp;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private const int STROKE_WIDTH = 50;
        private float offset;
        private float gradientCycleLength;
        private bool isAnimating;

        private SKRect pathBounds;
        private SKPath infinityPath;
        private SKColor[] colors;
        private Stopwatch stopwatch;

        public RegisterPage()
        {
            InitializeComponent();
            colors = new SKColor[3];
            colors[0] = SKColor.Parse("FFC3A0");
            colors[1] = SKColor.Parse("FFAFBD");
            colors[2] = SKColor.Parse("FFC3A0");


            infinityPath = new SKPath();
            infinityPath.MoveTo(0, 0);           
            infinityPath.LineTo(1000f, 0);
            infinityPath.LineTo(1000f, 1000f);
            infinityPath.LineTo(0, 1000f);
            infinityPath.LineTo(0, 0);


            // Calculate path information 
            pathBounds = infinityPath.Bounds;
            gradientCycleLength = pathBounds.Width +
                pathBounds.Height * pathBounds.Height / pathBounds.Width;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            isAnimating = true;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            Device.StartTimer(TimeSpan.FromMilliseconds(20), OnTimerTick);
        }

        private void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout st = sender as StackLayout;
            st.WidthRequest = Width * 0.3;
        }

        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            //Set transforms to shift path to center and scale to canvas size
            canvas.Translate(info.Width / 2, info.Height / 2);
            canvas.Scale(0.95f *
                Math.Min(info.Width / (pathBounds.Width + STROKE_WIDTH),
                         info.Height / (pathBounds.Height + STROKE_WIDTH)));


           
            using (SKPaint paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = STROKE_WIDTH;
                paint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(pathBounds.Left, pathBounds.Top),
                                    new SKPoint(pathBounds.Right, pathBounds.Bottom),
                                    colors,
                                    null,
                                    SKShaderTileMode.Repeat,
                                    SKMatrix.MakeTranslation(offset, 0));

                canvas.DrawPaint(paint);
            }
        }

        private bool OnTimerTick()
        {
            const int duration = 2;     // seconds
            double progress = stopwatch.Elapsed.TotalSeconds % duration / duration;
            offset = (float)(gradientCycleLength * progress);
            canvasView.InvalidateSurface();

            return isAnimating;
        }
    }
}