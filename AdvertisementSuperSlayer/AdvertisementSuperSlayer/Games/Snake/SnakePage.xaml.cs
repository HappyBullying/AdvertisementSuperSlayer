
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using AdvertisementSuperSlayer.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Diagnostics;

namespace AdvertisementSuperSlayer.Games.Snake
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SnakePage : ContentPage
    {
        List<SnakePoint> points;
        string direction = "right";
        SKBitmap open = BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.open400x612.png");
        private float dxdy = 40;
        public SnakePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            SKCanvasView canvasView = new SKCanvasView();
            canvasView.WidthRequest = canvasView.Height;
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
            points = new List<SnakePoint>();
            points.Add(new SnakePoint(10, 10));
            points.Add(new SnakePoint(10, 50));
            points.Add(new SnakePoint(10, 90));
            points.Add(new SnakePoint(50, 90));
            points.Add(new SnakePoint(90, 90));
            points.Add(new SnakePoint(90, 90));
            points.Add(new SnakePoint(90, 90));
            points.Add(new SnakePoint(90, 90));
            points.Add(new SnakePoint(90, 90));
            points.Add(new SnakePoint(90, 90));
            points.Add(new SnakePoint(90, 90));
            points.Reverse();
            Device.StartTimer(TimeSpan.FromMilliseconds(150), () =>
            {
                for (int i = points.Count - 1; i > 0; i--)
                {
                    points[i].X = points[i - 1].X;
                    points[i].Y = points[i - 1].Y;
                }
                if (direction == "up")
                {
                    //points.Insert(0, new Point(points[0].X, points[0].Y - dxdy));
                    //points.RemoveAt(points.Count - 1);


                    
                    points[0].Y = points[0].Y - dxdy;
                    canvasView.InvalidateSurface();
                    return true;
                }
                if (direction == "down")
                {
                    //points.Insert(0, new Point(points[0].X, points[0].Y + dxdy));
                    //points.RemoveAt(points.Count - 1);
                    points[0].Y = points[0].Y + dxdy;


                    canvasView.InvalidateSurface();
                    return true;
                }
                if (direction == "left")
                {
                    //points.Insert(0, new Point(points[0].X - dxdy, points[0].Y));
                    //points.RemoveAt(points.Count - 1);
                    points[0]. X= points[0].X - dxdy;

                    canvasView.InvalidateSurface();
                    return true;
                }
                else
                {
                    //points.Insert(0, new Point(points[0].X + dxdy, points[0].Y));
                    //points.RemoveAt(points.Count - 1);
                    points[0].X = points[0].X + dxdy;

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

            //SKPaint kPaint = new SKPaint();
            //kPaint.Color = SKColors.Red;
            //kPaint.StrokeWidth = 5;
            //kPaint.Style = SKPaintStyle.Stroke;
            //SKImageInfo sK = new SKImageInfo(90, 137);
            //open = open.Resize(sK, SKFilterQuality.High);
            //canvas.DrawPoint(100, 100, kPaint);
            //canvas.RotateDegrees(-90, 100, 100);
            //canvas.DrawBitmap(open, 100, 100);






            SKPaint thick = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                
                Color = SKColors.Purple,
                StrokeWidth = 5,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };
            //canvas.DrawCircle(500, 500, 100, thick);

            int maxW = e.Info.Width;
            int maxH = e.Info.Height;

            //SKStrokeJoin strokeJoin = SKStrokeJoin.Round;
            //SKPath path = new SKPath();
            //path.MoveTo(new SKPoint((float)points[points.Count - 1].X, (float)points[points.Count - 1].Y));

            foreach(SnakePoint pt in points)
            {
                canvas.DrawCircle(pt.X, pt.Y, 30, thick);
            }


            //thick.StrokeJoin = strokeJoin;
            //canvas.DrawPath(path, thick);

            SKImageInfo sKImageInfo = new SKImageInfo(60, 82);
            open = open.Resize(sKImageInfo, SKFilterQuality.High);
            if (direction == "up")
            {
                //canvas.Translate((float)points[0].X, ((float)points[0].Y));
                //canvas.Scale(0.2f);
                //canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                canvas.RotateDegrees(180, points[0].X, points[0].Y);
                canvas.DrawBitmap(open, points[0].X - open.Width / 2, points[0].Y, thick);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "down")
            {
                //canvas.Translate((float)points[0].X, (float)(points[0].Y));
                //canvas.Scale(0.2f);
                
                canvas.DrawBitmap(open, points[0].X - open.Width / 2, points[0].Y, thick);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "left")
            {
                //canvas.Translate((float)(points[0].X), (float)points[0].Y);
                //canvas.Scale(0.2f);
                //canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                canvas.RotateDegrees(90, points[0].X, points[0].Y);
                canvas.DrawBitmap(open, points[0].X - open.Width / 2, points[0].Y, thick);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "right")
            {
                //canvas.Translate((float)(points[0].X), (float)points[0].Y);
                //canvas.Scale(0.2f);
                //canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                
                canvas.RotateDegrees(-90, points[0].X, points[0].Y);
                canvas.DrawBitmap(open, points[0].X - open.Width / 2, points[0].Y, thick);
                //canvas.Scale(5f);
                return;
            }
        }
    }
}