
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using AdvertisementSuperSlayer.Helpers;
using System.Collections.Generic;
using System;

namespace AdvertisementSuperSlayer.Games.Snake
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SnakePage : ContentPage
    {
        List<Point> points;
        string direction = "right";
        SKBitmap open = BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.openb.png");
       
        public SnakePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;


            SwipeGestureRecognizer up = new SwipeGestureRecognizer();
            up.Direction = SwipeDirection.Up;
            up.Swiped += (s, e) =>
            {
                direction = "up";
            };


            SwipeGestureRecognizer down = new SwipeGestureRecognizer();
            down.Direction = SwipeDirection.Down;
            down.Swiped += (s, e) =>
            {
                direction = "down";
            };


            SwipeGestureRecognizer left = new SwipeGestureRecognizer();
            left.Direction = SwipeDirection.Left;
            left.Swiped += (s, e) =>
            {
                direction = "left";
            };


            SwipeGestureRecognizer right = new SwipeGestureRecognizer();
            right.Direction = SwipeDirection.Right;
            right.Swiped += (s, e) =>
            {
                direction = "right";
            };
            
            canvasView.GestureRecognizers.Add(up);
            canvasView.GestureRecognizers.Add(down);
            canvasView.GestureRecognizers.Add(left);
            canvasView.GestureRecognizers.Add(right);
            this.Content = canvasView;
            DependencyService.Get<IAudio>().SetupAudioFile("woo.mp3");
            DependencyService.Get<IAudio>().PlaySound();
            points = new List<Point>();
            points.Add(new Point(10, 10));
            points.Add(new Point(10, 50));
            points.Add(new Point(10, 90));
            points.Add(new Point(50, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));


            Device.StartTimer(TimeSpan.FromMilliseconds(80), ()=>{
                if (direction == "up")
                {
                    points.Insert(0, new Point(points[0].X, points[0].Y - 40));
                    
                    points.RemoveAt(points.Count - 1);
                    canvasView.InvalidateSurface();
                    return true;
                }
                if (direction == "down")
                {
                    points.Insert(0, new Point(points[0].X, points[0].Y + 40));
                    points.RemoveAt(points.Count - 1);
                    canvasView.InvalidateSurface();
                    return true;
                }
                if (direction == "left")
                {
                    points.Insert(0, new Point(points[0].X - 40, points[0].Y));
                    points.RemoveAt(points.Count - 1);
                    canvasView.InvalidateSurface();
                    return true;
                }
                else
                {
                    points.Insert(0, new Point(points[0].X + 40, points[0].Y));
                    points.RemoveAt(points.Count - 1);
                    canvasView.InvalidateSurface();
                    return true;
                }
            });
        }
       
        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            
            SKPaint thick = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Purple,
                StrokeWidth = 10,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };

            int maxW = e.Info.Width;
            int maxH = e.Info.Height;

            SKStrokeJoin strokeJoin = SKStrokeJoin.Round;
            SKPath path = new SKPath();
            path.MoveTo(new SKPoint((float)points[points.Count - 1].X, (float)points[points.Count - 1].Y));

            for (int i = points.Count - 2; i > 0; i--)
            {
                path.LineTo(new SKPoint((float)points[i].X, (float)points[i].Y));
            }

            
            thick.StrokeJoin = strokeJoin;
            canvas.DrawPath(path, thick);

            SKImageInfo sKImageInfo = new SKImageInfo(125, 190);
            //open = open.Resize(sKImageInfo, SKFilterQuality.High);
            if (direction == "up")
            {
                canvas.Translate((float)points[0].X, ((float)points[0].Y) );
                canvas.Scale(0.2f);
                canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "down")
            {
                canvas.Translate((float)points[0].X, (float)(points[0].Y));
                canvas.Scale(0.2f);
                canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "left")
            {
                canvas.Translate((float)(points[0].X), (float)points[0].Y);
                canvas.Scale(0.2f);
                canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "right")
            {
                canvas.Translate((float)(points[0].X), (float)points[0].Y);
                canvas.Scale(0.2f);
                canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                //canvas.Scale(5f);
                return;
            }
        }

    }
}