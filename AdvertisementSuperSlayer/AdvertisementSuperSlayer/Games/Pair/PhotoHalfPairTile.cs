using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Games.Pair
{
    class PhotoHalfPairTile : ContentView
    {
        private readonly SKBitmap TileBitmap;
        private readonly SKBitmap FrontFaceBitmap;
        private SKBitmap CurrentBitmap;
        private SKCanvasView canvasView;
        private bool IsTile = true;
        private float _Deg = 0;



        public bool Flipped { get; set; }
        public int Row { set; get; }
        public int Col { set; get; }
        public bool IsRotating { get; set; }
        public string FrontBitmapName { get; set; }
        public bool WasTapped { get; set; }
        public PhotoHalfPairTile(SKBitmap tile, SKBitmap frontface)
        {
            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            TileBitmap = tile;
            SKImageInfo imageInfo = new SKImageInfo(250, 250);
            FrontFaceBitmap = frontface;
            Content = canvasView;
            CurrentBitmap = TileBitmap;
            canvasView.InvalidateSurface();
            IsRotating = false;
            WasTapped = false;
        }


        public void InvalidSurfaceState() { this.canvasView.InvalidateSurface(); }

        public void AddGestureRecognizer(TapGestureRecognizer rc)
        {
            canvasView.GestureRecognizers.Add(rc);
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
            perspectiveMatrix[3, 2] = -1 / 5000f;
            matrix44.PostConcat(perspectiveMatrix);

            SKMatrix.PostConcat(ref matrix, matrix44.Matrix);
            SKMatrix.PostConcat(ref matrix, SKMatrix.MakeTranslation(xCenter, yCenter));
            canvas.SetMatrix(matrix);
            float xBitmap = xCenter - CurrentBitmap.Width / 2;
            float yBitmap = yCenter - CurrentBitmap.Height / 2;
            canvas.DrawBitmap(CurrentBitmap, xBitmap, yBitmap);
        }


        public void SetOk(SKBitmap ok)
        {
            CurrentBitmap = ok;
            canvasView.InvalidateSurface();
        }

        public float Deg
        {
            get
            {
                return this._Deg;
            }
            set
            {
                if (value == 90.0f)
                {
                    if (IsTile)
                    {
                        IsTile = !IsTile;
                        CurrentBitmap = FrontFaceBitmap;
                    }
                    else
                    {
                        IsTile = !IsTile;
                        CurrentBitmap = TileBitmap;
                    }
                }
                _Deg = value;
                canvasView.InvalidateSurface();
            }
        }
    }
}
