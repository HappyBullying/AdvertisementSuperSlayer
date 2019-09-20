
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using AdvertisementSuperSlayer.Helpers;

namespace AdvertisementSuperSlayer.Games.Snake
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SnakePage : ContentPage
    {
        public SnakePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            this.Content = canvasView;
            DependencyService.Get<IAudio>().SetupAudioFile("woo.mp3");
            DependencyService.Get<IAudio>().PlaySound();

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
                Color = SKColors.Gold,
                StrokeWidth = 50,
                StrokeCap = SKStrokeCap.Round,
                IsAntialias = true
            };

            int maxW = e.Info.Width - 150;
            int maxH = e.Info.Height - 100;

            SKStrokeJoin strokeJoin = SKStrokeJoin.Round;
            SKPath path = new SKPath();
            path.MoveTo(60, 60);

            for (int i = 61, j = 61; i < maxW && j < maxH; i += 2, j++)
            {
                path.LineTo(i, j);
            }


            thick.StrokeJoin = strokeJoin;
            canvas.DrawPath(path, thick);
        }

    }
}