using AdvertisementSuperSlayer.Helpers;
using AdvertisementSuperSlayer.TouchTracking;
using AdvertisementSuperSlayer.TouchTracking.Transforms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using AdvertisementSuperSlayer.DbModels;
using System.Linq;
using System.Reflection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AdvertisementSuperSlayer.Browser;

namespace AdvertisementSuperSlayer.Games.Puzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PuzzlePage : ContentPage, ISaveResult
    {
        private List<TouchManipulationBitmap> bitmapCollection;
        private Dictionary<long, TouchManipulationBitmap> bitmapDictionary = new Dictionary<long, TouchManipulationBitmap>();
        private List<SKBitmap> Bitmaps = new List<SKBitmap>();
        private List<SKPoint> BoundPoints = new List<SKPoint>();
        private Stopwatch Timer;
        private Random rnd;
        private int Rows;
        private int Cols;
        private int QuadSize;
        private int Count;
        private double ActualWidth;
        private double ActualHeight;
        private bool canProcessTouchEvent = false;
        private bool timerIsVisible = true;
        private const float StartOffset = 50;
        private bool picturesReady = false;

        public PuzzlePage(int rows)
        {
            InitializeComponent();
            Rows = rows;
            Cols = (int)(rows * 1.5);
            rnd = new Random(DateTime.Now.Millisecond);
        }


        private async void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            if (!canProcessTouchEvent)
            {
                return;
            }
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
                        bool check = CheckRightCombination();
                        canvasView.InvalidateSurface();
                        if (check)
                        {
                            await DisplayAlert("Victory", "You have won", "Ok");
                            SaveResult();
                            await Navigation.PopAsync();
                        }
                    }
                    break;
            }
        }



        private void CheckAndMoveLanding(TouchManipulationBitmap bmp)
        {
            float xcol = (bmp.Matrix.TransX - StartOffset) / QuadSize;
            float yrow = (bmp.Matrix.TransY - StartOffset) / QuadSize;

            int col = (int)Math.Round(xcol);
            int row = (int)Math.Round(yrow);

            if (!(col > Cols + 1 || col < -1 || row > row + 1 || row < -1))
            {
                if (col > Cols - 1) { col = Cols - 1; }
                if (col < 0) { col = 0; }
                if (row > Rows - 1) { row = Rows - 1; }
                if (row < 0) { row = 0; }
                SKPoint pt = BoundPoints[row * Cols + col];
                bmp.Matrix = SKMatrix.MakeTranslation(pt.X, pt.Y);
            }
        }



        private bool CheckRightCombination()
        {
            List<SKPoint> pts = new List<SKPoint>();
            for (int i = 0; i < bitmapCollection.Count; i++)
            {
                pts.Add(new SKPoint { X = bitmapCollection[i].Matrix.TransX, Y = bitmapCollection[i].Matrix.TransY });
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    int xI = (int)Math.Round(bitmapCollection[i * Cols + j].Matrix.TransX);
                    int yI = (int)Math.Round(bitmapCollection[i * Cols + j].Matrix.TransY);

                    int index = bitmapCollection[i * Cols + j].ImageId;

                    int xR = (int)Math.Round(BoundPoints[index].X);
                    int yR = (int)Math.Round(BoundPoints[index].Y);

                    if (!(xI == xR && yI == yR))
                    {
                        return false;
                    }
                }

            }
            return true;
        }







        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();
            DrawGrid(canvas);
            DrawCounter(canvas);

            foreach (TouchManipulationBitmap bitmap in bitmapCollection)
            {
                SKPoint pt = new SKPoint(bitmap.Matrix.TransX, bitmap.Matrix.TransY);
                canvas.DrawBitmap(bitmap.bitmap, pt);
            }
        }

        private void DrawCounter(SKCanvas canvas)
        {
            if (timerIsVisible)
            {

                const float offset = 30;
                const float height = 100;
                const float width = 80;
                SKPaint paint = new SKPaint
                {
                    Color = SKColors.Orange,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    StrokeWidth = 4,
                    TextSize = height
                };
                canvas.DrawText(Count.ToString(), (float)(ActualWidth - 2 * width - offset), height, paint);
            }
        }


        private void DrawGrid(SKCanvas canvas)
        {
            int thikness = 5;
            // Draw grid
            float leftTopX = BoundPoints[0].X - 2;
            float leftTopY = BoundPoints[0].Y - 2;
            float width = BoundPoints[BoundPoints.Count - 1].X - leftTopX + QuadSize + 2;
            float height = BoundPoints[BoundPoints.Count - 1].Y - leftTopY + QuadSize + 2;

            SKPaint paint = new SKPaint
            {
                Color = SKColors.Gray,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = thikness
            };
            canvas.DrawRect(leftTopX, leftTopY, width, height, paint);



            paint.StrokeWidth = 2;
            int top = 0;
            int bot = BoundPoints.Count - Cols;
            for (int i = 0; i < Cols; i++)
            {
                SKPoint pt = new SKPoint(BoundPoints[bot + i].X, BoundPoints[bot + i].Y + QuadSize);
                canvas.DrawLine(BoundPoints[top + i], pt, paint);
            }
            int left = 0;
            int right = Cols - 1;
            for (int i = 0; i < Rows; i++)
            {
                SKPoint pt = new SKPoint(BoundPoints[right + i * Cols].X + QuadSize, BoundPoints[right + i * Cols].Y);
                canvas.DrawLine(BoundPoints[left + i * Cols], pt, paint);
            }
        }



        private void StartGame()
        {
            timerIsVisible = true;
            Count = 201;
            Device.StartTimer(TimeSpan.FromSeconds(1), GameTimerCallback);
            Timer = Stopwatch.StartNew();
        }


        private bool GameTimerCallback()
        {
            Count--;
            if (Count < 0)
            {
                timerIsVisible = false;
                canvasView.InvalidateSurface();
                DisplayAlert("Defeat", "You have lost", "Ok").GetAwaiter().OnCompleted(async () => {
                    if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                    {
                        await Navigation.PushAsync(new AdvPage());
                    }
                    else
                    {
                        BrowserPage browser = new BrowserPage();
                        await Navigation.PushAsync(browser);
                        browser.Navigate(App.Rest.advUrl);
                    }
                });
                
                return false;
            }
            else
            {
                canvasView.InvalidateSurface();
                return true;
            }
        }




        private void ShowInitialImage()
        {
            Count = 10;
            Device.StartTimer(TimeSpan.FromSeconds(1), InitialImageCallback);
        }


        private bool InitialImageCallback()
        {
            canvasView.InvalidateSurface();
            Count--;
            if (Count < 0)
            {
                MixImages();
                canProcessTouchEvent = true;
                timerIsVisible = false;
                canvasView.InvalidateSurface();
                StartGame();
                return false;
            }

            return true;
        }


        private void MixImages()
        {
            List<SKMatrix> matrixes = new List<SKMatrix>();
            for (int i = 0; i < bitmapCollection.Count; i++)
            {
                matrixes.Add(bitmapCollection[i].Matrix);
            }
            for (int i = 0; i < bitmapCollection.Count; i++)
            {
                int randVal = rnd.Next(matrixes.Count);
                bitmapCollection[i].Matrix = matrixes[randVal];
                matrixes.RemoveAt(randVal);
            }
        }


        private void canvasView_SizeChanged(object sender, EventArgs e)
        {
            Xamarin.Essentials.DisplayInfo DInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            double scale = DInfo.Width / Width;
            ActualHeight = Height * scale;
            ActualWidth = Width * scale;

            QuadSize = (int)(ActualHeight * 0.8 / Rows);
            if (!picturesReady)
            {
                picturesReady = true;
                InitializePictures();
                ShowInitialImage();
            }
        }



        private void InitializePictures()
        {
            // Load bitmaps
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] resourceIDs = (from tmp in assembly.GetManifestResourceNames()
                                    where tmp.Contains(".Puzzle.")
                                    select tmp).ToArray();

            SKImageInfo inf = new SKImageInfo((int)(ActualHeight * 1.2), (int)(ActualHeight * 0.8));

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

        public void SaveResult()
        {
            Timer.Stop();
            PuzzleRecord record = new PuzzleRecord
            {
                GameTime = Timer.ElapsedMilliseconds,
                LastModified = DateTime.UtcNow
            };

            App.Rest.UpdatePuzzle(record);
        }
    }
}