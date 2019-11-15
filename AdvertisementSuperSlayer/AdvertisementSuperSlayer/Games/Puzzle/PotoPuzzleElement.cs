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
        public string PuzzleId { get; set; }
        public float Progress { get; set; }
        bool IsSK = false;
        SKRect rect;
        public PhotoPuzzleElement()
        {
            canvasView = new SKCanvasView();
            Content = canvasView;
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Progress = 0;
            
        }

        public void InvalidSurfaceState()
        {
            canvasView.InvalidateSurface();
        }


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            if (!IsSK)
            {
                IsSK = true;
                // Find rectangle to fit bitmap
                float scale = Math.Min((float)info.Width / bitmap1.Width,
                                       (float)info.Height / bitmap1.Height);
                rect = SKRect.Create(scale * bitmap1.Width,
                                            scale * bitmap1.Height);
                float x = (info.Width - rect.Width) / 2;
                float y = (info.Height - rect.Height) / 2;
                rect.Offset(x, y);
            }
            // Get progress value from Slider

            // Display two bitmaps with transparency
            using (SKPaint paint = new SKPaint { IsAntialias = true })
            {
                canvas.DrawBitmap(bitmap1, rect, paint);
            }
        }
    }
}
