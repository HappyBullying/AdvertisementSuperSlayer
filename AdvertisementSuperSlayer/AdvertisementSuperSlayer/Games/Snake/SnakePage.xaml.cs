
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using AdvertisementSuperSlayer.Helpers;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AdvertisementSuperSlayer.Games.Snake
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SnakePage : ContentPage
    {
        List<Point> points;
        string direction = "right";
        //SKBitmap open = BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.openb.png");
        private double dxdy = 5;
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
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));
            points.Add(new Point(90, 90));

            Device.StartTimer(TimeSpan.FromMilliseconds(5), () =>
            {
                if (direction == "up")
                {
                    points.Insert(0, new Point(points[0].X, points[0].Y - dxdy));
                    points.RemoveAt(points.Count - 1);

                    //points[0] = new Point(points[0].X, points[0].Y - dxdy);
                    //for (int i = 1; i < points.Count; i++)
                    //{
                    //    double dX = points[i - 1].X - points[i].X;
                    //    double dY = points[i - 1].Y - points[i].Y;

                    //    if (dX == 0)
                    //    {
                    //        if (dY > 0)
                    //        {
                    //            points[i] = new Point(points[i].X, points[i].Y + 50);
                    //        }
                    //        else
                    //        {
                    //            points[i] = new Point(points[i].X, points[i].Y - 50);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (dX > 0)
                    //        {
                    //            points[i] = new Point(points[i].X + 50, points[i].Y);
                    //        }
                    //        else
                    //        {
                    //            points[i] = new Point(points[i].X - 50, points[i].Y);
                    //        }
                    //    }
                    //}

                    canvasView.InvalidateSurface();
                    return true;
                }
                if (direction == "down")
                {
                    points.Insert(0, new Point(points[0].X, points[0].Y + dxdy));
                    points.RemoveAt(points.Count - 1);

                    //points[0] = new Point(points[0].X, points[0].Y + dxdy);
                    //for (int i = 1; i < points.Count; i++)
                    //{
                    //    double dX = points[i - 1].X - points[i].X;
                    //    double dY = points[i - 1].Y - points[i].Y;

                    //    if (dX == 0)
                    //    {
                    //        if (dY > 0)
                    //        {
                    //            points[i] = new Point(points[i].X, points[i].Y + 50);
                    //        }
                    //        else
                    //        {
                    //            points[i] = new Point(points[i].X, points[i].Y - 50);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (dX > 0)
                    //        {
                    //            points[i] = new Point(points[i].X + 50, points[i].Y);
                    //        }
                    //        else
                    //        {
                    //            points[i] = new Point(points[i].X - 50, points[i].Y);
                    //        }
                    //    }
                    //}

                    canvasView.InvalidateSurface();
                    return true;
                }
                if (direction == "left")
                {
                    points.Insert(0, new Point(points[0].X - dxdy, points[0].Y));
                    points.RemoveAt(points.Count - 1);

                    //points[0] = new Point(points[0].X - dxdy, points[0].Y);
                    //for (int i = 1; i < points.Count; i++)
                    //{
                    //    double dX = points[i - 1].X - points[i].X;
                    //    double dY = points[i - 1].Y - points[i].Y;

                    //    if (dX == 0)
                    //    {
                    //        if (dY > 0)
                    //        {
                    //            points[i] = new Point(points[i].X, points[i].Y + 50);
                    //        }
                    //        else
                    //        {
                    //            points[i] = new Point(points[i].X, points[i].Y - 50);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (dX > 0)
                    //        {
                    //            points[i] = new Point(points[i].X + 50, points[i].Y);
                    //        }
                    //        else
                    //        {
                    //            points[i] = new Point(points[i].X - 50, points[i].Y);
                    //        }
                    //    }
                    //}

                    canvasView.InvalidateSurface();
                    return true;
                }
                else
                {
                    points.Insert(0, new Point(points[0].X + dxdy, points[0].Y));
                    points.RemoveAt(points.Count - 1);

                    //points[0] = new Point(points[0].X + dxdy, points[0].Y);
                    //for (int i = 1; i < points.Count; i++)
                    //{
                    //    double dX = points[i - 1].X - points[i].X;
                    //    double dY = points[i - 1].Y - points[i].Y;

                    //    if (dX == 0)
                    //    {
                    //        if (dY > 0)
                    //        {
                    //            points[i] = new Point(points[i].X, points[i].Y + 50);
                    //        }
                    //        else
                    //        {
                    //            points[i] = new Point(points[i].X, points[i].Y - 50);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (dX > 0)
                    //        {
                    //            points[i] = new Point(points[i].X + 50, points[i].Y);
                    //        }
                    //        else
                    //        {
                    //            points[i] = new Point(points[i].X - dxdy, points[i].Y);
                    //        }
                    //    }
                    //}

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
                canvas.Translate((float)points[0].X, ((float)points[0].Y));
                canvas.Scale(0.2f);
                //canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "down")
            {
                canvas.Translate((float)points[0].X, (float)(points[0].Y));
                canvas.Scale(0.2f);
                //canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "left")
            {
                canvas.Translate((float)(points[0].X), (float)points[0].Y);
                canvas.Scale(0.2f);
                //canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                //canvas.Scale(5f);
                return;
            }
            if (direction == "right")
            {
                canvas.Translate((float)(points[0].X), (float)points[0].Y);
                canvas.Scale(0.2f);
                //canvas.DrawBitmap(open, e.Info.Rect, BitmapStretch.Uniform);
                //canvas.Scale(5f);
                return;
            }
        }
    }
}