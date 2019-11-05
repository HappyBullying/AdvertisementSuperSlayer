using AdvertisementSuperSlayer.Games.Puzzle.PuzzleElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AdvertisementSuperSlayer.Helpers.TouchEffectHelpers;
using SkiaSharp.Views.Forms;

namespace AdvertisementSuperSlayer.Games.Puzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PuzzlePage : ContentPage
    {
        private List<PuzzleImageElement> PuzzleElements;
        private Dictionary<long, PuzzleImageElement> PzElemDictionary;
        public PuzzlePage()
        {
            InitializeComponent();
            PuzzleElements = new List<PuzzleImageElement>();
            PzElemDictionary = new Dictionary<long, PuzzleImageElement>();


            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] ResourceIds = assembly.GetManifestResourceNames();
            SKPoint position = new SKPoint();

            SKImageInfo inf = new SKImageInfo(200, 200);
            foreach (string rId in ResourceIds)
            {
                if (rId.Contains("Front"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(rId))
                    {
                        SKBitmap bitmap = SKBitmap.Decode(stream).Resize(inf, SKFilterQuality.High);
                        PuzzleElements.Add(new PuzzleImageElement(bitmap)
                        {
                            Matrix = SKMatrix.MakeTranslation(position.X, position.Y)
                        });
                        position.X += 80;
                        position.Y += 80;
                    }
                }
            }
            TouchEffect effect = new TouchEffect();
            effect.Capture = true;
            effect.TouchAction += this.OnTouchEffectAction;
            mGrid.Effects.Add(effect);
        }


        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            Point pt = args.Location;

            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    {
                        for (int i = PuzzleElements.Count - 1; i >= 0; i--)
                        {
                            PuzzleImageElement bitmap = PuzzleElements[i];
                            if (bitmap.HitTest(point))
                            {
                                // To the end
                                PuzzleElements.RemoveAt(i);
                                PuzzleElements.Add(bitmap);
                                // Do the touch processing
                                PzElemDictionary.Add(args.Id, bitmap);
                                bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                                canvasView.InvalidateSurface();
                                break;
                            }
                        }
                        break;
                    }

                case TouchActionType.Moved:
                    {
                        if (PzElemDictionary.ContainsKey(args.Id))
                        {
                            PuzzleImageElement bitmap = PzElemDictionary[args.Id];
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            canvasView.InvalidateSurface();
                        }
                        break;
                    }

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (PzElemDictionary.ContainsKey(args.Id))
                    {
                        PuzzleImageElement bitmap = PzElemDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        PzElemDictionary.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();

            foreach (PuzzleImageElement element in PuzzleElements)
            {
                element.Paint(canvas);
            }
        }

    }
}