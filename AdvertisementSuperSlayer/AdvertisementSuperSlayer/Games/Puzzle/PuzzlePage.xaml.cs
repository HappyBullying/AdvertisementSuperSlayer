using AdvertisementSuperSlayer.TouchTracking;
using AdvertisementSuperSlayer.TouchTracking.Transforms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.Puzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PuzzlePage : ContentPage
    {
        private List<TouchManipulationBitmap> bitmapCollection;
        private Dictionary<long, TouchManipulationBitmap> bitmapDictionary = new Dictionary<long, TouchManipulationBitmap>();
        private List<SKBitmap> Bitmaps = new List<SKBitmap>();
        private List<SKPoint> BoundPoints = new List<SKPoint>();
        private Random rnd;
        private int Rows;
        private int Cols;
        private int QuadSize;
        private const float StartOffset = 50;
        private bool picturesReady = false;
        
        public PuzzlePage(int rows)
        {
            InitializeComponent();
            Rows = rows;
            Cols = (int)(rows * 1.5);
            rnd = new Random(DateTime.Now.Millisecond);
        }


        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    for (int i = bitmapCollection.Count - 1; i >= 0; i--)
                    {
                        TouchManipulationBitmap bitmap = bitmapCollection[i];

                        if (bitmap.HitTest(point))
                        {
                            // Move bitmap to end of collection
                            bitmapCollection.Remove(bitmap);
                            bitmapCollection.Add(bitmap);
                            // Do the touch processing
                            bitmapDictionary.Add(args.Id, bitmap);
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            canvasView.InvalidateSurface();
                            break;
                        }
                    }
                    break;

                case TouchActionType.Moved:
                    if (bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        bitmapDictionary.Remove(args.Id);
                        CheckAndMoveLanding(bitmap);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }



        private void CheckAndMoveLanding(TouchManipulationBitmap bmp)
        {
            float xcol = (bmp.Matrix.TransX) / QuadSize;
            float yrow = (bmp.Matrix.TransY) / QuadSize;

            int col = (int)Math.Round(xcol);
            int row = (int)Math.Round(yrow);

            if (col > Cols - 1) { col = Cols - 1; }
            if (col < 0) { col = 0; }
            if(row > Rows - 1) { row = Rows - 1; }
            if (row < 0) { row = 0; }


            SKPoint pt = BoundPoints[row * Cols + col];
            bmp.Matrix = SKMatrix.MakeTranslation(pt.X, pt.Y);
        }



        private void InitializePictures()
        {
            // Load bitmaps
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] resourceIDs = (from tmp in assembly.GetManifestResourceNames()
                                    where tmp.Contains(".Puzzle.")
                                    select tmp).ToArray();

            SKImageInfo inf = new SKImageInfo((int)(Height * 1.2), (int)(Height * 0.8));

            for (int i = 0; i < resourceIDs.Length; i++)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceIDs[i]))
                {

                    SKBitmap bitmap = SKBitmap.Decode(stream).Resize(inf, SKFilterQuality.High);
                    Bitmaps.Add(bitmap);
                }
            }




            inf = new SKImageInfo(QuadSize, QuadSize);
            int imageId = rnd.Next(Bitmaps.Count);
            SKBitmap chosen = Bitmaps[imageId];

            bitmapCollection = new List<TouchManipulationBitmap>();
            SKPoint position = new SKPoint(StartOffset, StartOffset);
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    SKBitmap bmpNew = new SKBitmap(QuadSize, QuadSize);
                    SKRect dest = new SKRect(0, 0, QuadSize, QuadSize);
                    SKRect source = new SKRect(col * QuadSize, row * QuadSize, (col + 1) * QuadSize, (row + 1) * QuadSize);

                    using (SKCanvas canvas = new SKCanvas(bmpNew))
                    {
                        canvas.Clear();
                        canvas.DrawBitmap(chosen, source, dest);
                    }
                    TouchManipulationBitmap tmpBmp = new TouchManipulationBitmap(bmpNew, row * Cols + col)
                    {
                        Matrix = SKMatrix.MakeTranslation(position.X, position.Y)
                    };
                    BoundPoints.Add(position);
                    if (col == Cols - 1)
                    {
                        position.Y += QuadSize;
                        position.X = StartOffset;
                    }
                    else
                    {
                        position.X += QuadSize;
                    }
                    bitmapCollection.Add(tmpBmp);
                }
            }
        }


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            int thikness = 5;
            // Draw grid
            float leftTopX = BoundPoints[0].X - thikness;
            float leftTopY = BoundPoints[0].Y - thikness;
            float width = BoundPoints[BoundPoints.Count - 1].X - leftTopX + QuadSize + thikness;
            float height = BoundPoints[BoundPoints.Count - 1].Y - leftTopY + QuadSize + thikness;

            SKPaint paint = new SKPaint
            {
                Color = SKColors.Gray,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = thikness
            };
            canvas.DrawRect(leftTopX, leftTopY, width, height, paint);

            foreach (TouchManipulationBitmap bitmap in bitmapCollection)
            {
                SKPoint pt = new SKPoint(bitmap.Matrix.TransX, bitmap.Matrix.TransY);
                canvas.DrawBitmap(bitmap.bitmap, pt);
            }
        }

        private void canvasView_SizeChanged(object sender, EventArgs e)
        {
            QuadSize = (int)(Height * 0.8 / Rows);
            if (!picturesReady)
            {
                picturesReady = true;
                InitializePictures();
            }
        }
    }
}