using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Extended = SkiaSharp.Extended.Svg;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.IO;
using System.Reflection;

namespace AdvertisementSuperSlayer.Games.SvgTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SVGPage : ContentPage
    {
        Extended.SKSvg svg;
        Extended.SKSvg svg1 = new Extended.SKSvg();
        Extended.SKSvg svg2 = new Extended.SKSvg();
        Extended.SKSvg svg3 = new Extended.SKSvg();
        Extended.SKSvg svg4 = new Extended.SKSvg();
        Stack<int> order = new Stack<int>();
        int current = 2;

        private float rt = 0.1f;

        public SVGPage()
        {
            InitializeComponent();
            svg = new Extended.SKSvg();
            Stream imgstream = GetImageStream(1);
            svg.Load(imgstream);
            imgstream.Close();

            imgstream = GetImageStream(1);
            svg1.Load(imgstream);
            imgstream.Close();
            imgstream = GetImageStream(2);
            svg2.Load(imgstream);
            imgstream.Close();
            imgstream = GetImageStream(3);
            svg3.Load(imgstream);
            imgstream.Close();
            imgstream = GetImageStream(4);
            svg4.Load(imgstream);
            imgstream.Close();

            order.Push(3);
            order.Push(4);
            order.Push(1);
            order.Push(2);

            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            this.Content = canvasView;
            Device.StartTimer(TimeSpan.FromMilliseconds(200), () =>
            {
                if (current == 1)
                {
                    current = 2;
                    svg = svg2;
                    canvasView.InvalidateSurface();
                    return true;
                }
                if (current == 2)
                {
                    current = 3;
                    svg = svg3;
                    canvasView.InvalidateSurface();
                    return true;
                }
                if (current == 3)
                {
                    current = 4;
                    svg = svg4;
                    canvasView.InvalidateSurface();
                    return true;
                }
                else
                {
                    current = 1;
                    svg = svg1;
                    canvasView.InvalidateSurface();
                    return true;
                }
            });

        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            //Stream imgstream = GetImageStream(current);
            //svg.Load(imgstream);
            //imgstream.Close();
            var surface = e.Surface;
            var canvas = surface.Canvas;
            var width = e.Info.Width;
            var height = e.Info.Height;
            canvas.Clear(SKColors.White);
            float canvasMin = Math.Min(width, height);
            
            float svgMax = Math.Max(svg.Picture.CullRect.Width, svg.Picture.CullRect.Height);
            float scale = canvasMin / svgMax;
            scale *= 1.3f;
            float ratioX = svg.Picture.CullRect.Width / svg.ViewBox.Width;
            float ratioY = svg.Picture.CullRect.Height / svg.ViewBox.Height;
            canvas.Scale(0.22f);
            canvas.Translate(e.Info.Width / 2, e.Info.Height / 2);
            canvas.RotateDegrees((float)(60 * Math.Sin(scale)));
            float rad = Math.Min(e.Info.Width, e.Info.Height) / 3;
            canvas.Translate(rad, 0);

            canvas.RotateDegrees((float)(60 * Math.Sin(scale)));

            canvas.DrawPicture(svg.Picture);
        }

        private Stream GetImageStream(string imgName)
        {
            string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(names[1]);
        }

        private Stream GetImageStream(int id)
        {
            //AdvertisementSuperSlayer.Resources.almost closed.svg
            string[] names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(names[id]);
        }
    }
}