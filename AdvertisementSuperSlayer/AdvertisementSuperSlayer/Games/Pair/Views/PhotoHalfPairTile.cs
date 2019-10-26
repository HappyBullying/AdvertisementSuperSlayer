using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Games.Pair.Views
{
    class PhotoHalfPairTile : ContentView
    {
        public static readonly BindableProperty DegProperty = BindableProperty.Create("Deg", typeof(float), null);

        public SKBitmap TileBitmap { get; set; }
        public SKBitmap CoverBitmap;
        private SKCanvasView canvasView;
        public PhotoHalfPairTile() { }
        public PhotoHalfPairTile(int row, int col, string bitmapPath, SKBitmap cover)
        {   
            Row = row;
            Col = col;
            Padding = new Thickness(5);
            canvasView = new SKCanvasView();
            Content = this.canvasView;
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(bitmapPath))
            {
                this.TileBitmap = SKBitmap.Decode(stream);
            }
            SKImageInfo sK = new SKImageInfo(100, 100);
            this.CoverBitmap = cover.Resize(sK, SKFilterQuality.High);
            this.canvasView.PaintSurface += this.OnCanvasViewPaintSurface;
        }


        public void InvalidSurfaceState() { this.canvasView.InvalidateSurface(); }

        public void Rotate(float deg)
        {
            _Deg = deg;
            InvalidSurfaceState();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            float xCenter = info.Width / 2;
            float yCenter = info.Height / 2;

            SKMatrix matrix = SKMatrix.MakeTranslation(-xCenter, -yCenter);
            SKMatrix44 matrix44 = SKMatrix44.CreateIdentity();
            matrix44.PostConcat(SKMatrix44.CreateRotationDegrees(1, 0, 0, _Deg));

            SKMatrix44 perspectiveMatrix = SKMatrix44.CreateIdentity();
            perspectiveMatrix[3, 2] = -1 / 2500f;
            matrix44.PostConcat(perspectiveMatrix);

            SKMatrix.PostConcat(ref matrix, matrix44.Matrix);
            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeTranslation(xCenter, yCenter));
            canvas.SetMatrix(matrix);
            float xBitmap = xCenter - CoverBitmap.Width / 2;
            float yBitmap = yCenter - CoverBitmap.Height / 2;
            canvas.DrawBitmap(CoverBitmap, xBitmap, yBitmap);
        }


        public int Row { set; get; }
        public int Col { set; get; }

        public float Deg
        {
            get
            {
                return this._Deg;
            }
            set
            {
                this._Deg = value;
                this.canvasView.InvalidateSurface();
            }
        }
        private float _Deg = 0;

    }
}
