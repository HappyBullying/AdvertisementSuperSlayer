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

        private SKImageInfo currentInfo;
        private readonly int Rows;
        private readonly int Cols;
        private double WindowWidth;
        private double WindowHeight;
        private int[,] CurrentPositions;
        private int[,] RightConfiguration;
        public static bool CanHandleEffect;
        private double PointDistance;



        private List<PuzzleImageElement> PuzzleElements;
        private Dictionary<long, PuzzleImageElement> PzElemDictionary;
        public PuzzlePage()
        {
            InitializeComponent();
            CanHandleEffect = true;
            PuzzleElements = new List<PuzzleImageElement>();
            PzElemDictionary = new Dictionary<long, PuzzleImageElement>();
            PointDistance = 30;
            int idCount = 0;
            Cols = 4;
            Rows = 2;



            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string[] ResourceIds = assembly.GetManifestResourceNames();
            SKPoint position = new SKPoint();
            int _id = 1;
            currentInfo = new SKImageInfo(150, 150);
            foreach (string rId in ResourceIds)
            {
                if (rId.Contains("Front"))
                {
                    using (Stream stream = assembly.GetManifestResourceStream(rId))
                    {
                        SKBitmap bitmap = SKBitmap.Decode(stream).Resize(currentInfo, SKFilterQuality.High);
                        PuzzleElements.Add(new PuzzleImageElement(bitmap)
                        {
                            Matrix = SKMatrix.MakeTranslation(position.X, position.Y),
                            ImageId = _id,
                            CurrentPosition = new SKPoint(position.X, position.Y)
                        });
                        _id++;
                        idCount++;
                        position.X += 170;
                        if (idCount == Cols)
                        {
                            position.Y += 170;
                            position.X = 0;
                        }
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

            if (CanHandleEffect)
            {
                CanHandleEffect = false;
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

                            /////Писать замену одного на другое здесь!!
                            bitmap.CurrentPosition = new SKPoint((float)args.Location.X, (float)args.Location.Y);

                            int foundIndex = CheckDistance(bitmap);
                            if (foundIndex != -1)
                            {
                                SKPoint insertionPoint =
                                    new SKPoint(PuzzleElements[foundIndex].CurrentPosition.X,
                                    PuzzleElements[foundIndex].CurrentPosition.Y);
                                bitmap.ProcessTouchEvent(args.Id, args.Type, insertionPoint);
                                PzElemDictionary.Clear();
                                canvasView.InvalidateSurface();
                                break;
                            }
                            bitmap.CurrentPosition = bitmap.GetBitmapLocation(bitmap.CurrentPosition);


                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            //PzElemDictionary.Remove(args.Id);
                            PzElemDictionary.Clear();
                            canvasView.InvalidateSurface();



                            

                            //for (int i = 0; i < CurrentPositions.Rank; i++)
                            //    for (int j = 0; j < CurrentPositions.Length; j++)
                            //    {
                            //        //if (CurrentPositions[i, j] == bitmap.ImageId)

                            //    }
                        }
                        break;
                    default:
                        break;
                }
                CanHandleEffect = true;
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


        private int CheckDistance(PuzzleImageElement pz)
        {
            double d1, d2, res;
            for (int i = 0; i < PuzzleElements.Count; i++)
            {
                if (PuzzleElements[i].ImageId != pz.ImageId)
                {
                    d1 = PuzzleElements[i].CurrentPosition.X - pz.CurrentPosition.X;
                    d2 = PuzzleElements[i].CurrentPosition.Y - pz.CurrentPosition.Y;
                    res = Math.Round(Math.Sqrt(d1 * d1 + d2 * d2));
                    if (res <= PointDistance)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }


        void OnContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            WindowWidth = contentView.Width;
            WindowHeight = contentView.Height;

            //if (WindowWidth <= 0 || WindowHeight <= 0)
            //    return;

            //// Orient StackLayout based on portrait/landscape mode.
            //stackLayout.Orientation = (WindowWidth < WindowHeight) ? StackOrientation.Vertical :
            //                                             StackOrientation.Horizontal;

            //// Calculate tile size and position based on ContentView size.
            //tileSize = Math.Min(WindowWidth, WindowHeight) / Math.Min(Rows, Cols);
            //absoluteLayout.WidthRequest = Cols * tileSize;
            //absoluteLayout.HeightRequest = Rows * tileSize;

            //foreach (View view in absoluteLayout.Children)
            //{
            //    PhotoHalfPairTile tile = (PhotoHalfPairTile)view;
            //    tile.InvalidSurfaceState();
            //    // Set tile bounds.
            //    AbsoluteLayout.SetLayoutBounds(tile,
            //        new Rectangle(tile.Col * tileSize, tile.Row * tileSize, tileSize, tileSize));
            //}
        }
    }
}