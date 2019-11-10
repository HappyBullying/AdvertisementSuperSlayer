using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using SkiaSharp;

namespace AdvertisementSuperSlayer.Games.Puzzle
{
    class PhotoPuzzleElement : ContentView
    {

        public int Row { get; set; }
        public int Col { get; set; }
        public SKBitmap bitmap1;
        public SKBitmap bitmap2;

        public SKCanvasView canvasView;

        public float Progress { get; set; }

        public PhotoPuzzleElement()
        {
            canvasView = new SKCanvasView();
            Content = canvasView;
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Margin = new Thickness(2);
            Progress = 0;
        }

        public void InvalidSurfaceState()
        {
            canvasView.InvalidateSurface();
        }

        public async Task TransitionFirstType(PhotoPuzzleElement first, PhotoPuzzleElement second, double time)
        {
            int delay_time = 18;
            first.bitmap2 = second.bitmap1;
            second.bitmap2 = first.bitmap1;
            float count = 0;
            for (count = 0; count < 1.0f; count += 2.0f)
            {
                await Task.Delay(delay_time);
                first.Progress = count;
                second.Progress = count;
                first.InvalidSurfaceState();
                second.InvalidSurfaceState();
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // Find rectangle to fit bitmap
            float scale = Math.Min((float)info.Width / bitmap1.Width,
                                   (float)info.Height / bitmap1.Height);
            SKRect rect = SKRect.Create(scale * bitmap1.Width,
                                        scale * bitmap1.Height);
            float x = (info.Width - rect.Width) / 2;
            float y = (info.Height - rect.Height) / 2;
            rect.Offset(x, y);

            // Get progress value from Slider

            // Display two bitmaps with transparency
            using (SKPaint paint = new SKPaint { IsAntialias = true })
            {
                paint.Color = paint.Color.WithAlpha((byte)(0xFF * (1 - Progress)));
                canvas.DrawBitmap(bitmap1, rect, paint);

                if (bitmap2 != null)
                {
                    paint.Color = paint.Color.WithAlpha((byte)(0xFF * Progress));
                    canvas.DrawBitmap(bitmap2, rect, paint);
                }
            }
        }
    }
}
