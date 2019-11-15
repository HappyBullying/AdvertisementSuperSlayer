using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.Puzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PuzzlePage : ContentPage
    {
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }

        private double tileSize;
        private readonly string pathToImages = "AdvertisementSuperSlayer.Images.";
        private int Rows;
        private int Cols;
        private PhotoPuzzleElement[][] PuzzleElements;
        private string[][] right_combo;

        private System.Diagnostics.Stopwatch stopwatch;
        private PhotoPuzzleElement first;
        private PhotoPuzzleElement second;



        public PuzzlePage(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Rows = 20;
            Cols = 32;
            InitializeComponent();
            Init();
            InitPuzzle(HandleImage(pathToImages + "Pair.Covers.green_full.png"));
            stopwatch.Stop();
            btn1.Text = stopwatch.ElapsedMilliseconds.ToString();
            // Find rectangle to fit bitmap



        }


        private void Init()
        {
            PuzzleElements = new PhotoPuzzleElement[Rows][];
            right_combo = new string[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                PuzzleElements[i] = new PhotoPuzzleElement[Cols];
                right_combo[i] = new string[Cols];
            }
        }

        private async void Click(object sender, EventArgs args)
        {
            if (!canExec)
                return;

            PhotoPuzzleElement pz = sender as PhotoPuzzleElement;
            if (first == null)
            {
                first = pz;
                pz.BackgroundColor = Color.Black;
                return;
            }
            if (second == null)
            {
                canExec = false;
                second = pz;
                pz.BackgroundColor = Color.Black;
                first.bitmap2 = second.bitmap1;
                second.bitmap2 = first.bitmap1;
            }

            PhotoPuzzleElement _t1_ = first;
            PhotoPuzzleElement _t2_ = second;
            await Task.Run(async () =>
            {
                for (int i = 0; i < 100; i += 6)
                {
                    await Task.Delay(18);
                    float prog = ((float)i) / 100.0f;
                    _t1_.Progress = prog;
                    _t2_.Progress = prog;

                    _t1_.InvalidSurfaceState();
                    _t2_.InvalidSurfaceState();
                }


                string tmp = _t1_.PuzzleId;
                _t1_.PuzzleId = _t2_.PuzzleId;
                _t2_.PuzzleId = tmp;

                _t1_.bitmap1 = _t1_.bitmap2;
                _t2_.bitmap1 = _t2_.bitmap2;

                _t1_.bitmap2 = null;
                _t2_.bitmap2 = null;

                _t1_.Progress = 0;
                _t2_.Progress = 0;
                _t1_.InvalidSurfaceState();
                _t2_.InvalidSurfaceState();



                Device.BeginInvokeOnMainThread(() =>
                {
                    _t1_.BackgroundColor = Color.White;
                    _t2_.BackgroundColor = Color.White;

                    _t1_ = null;
                    _t2_ = null;
                });

                SetTrue();
            });

            first = null;
            second = null;
        }


        void OnContentViewSizeChanged(object sender, EventArgs args)
        {
            ContentView contentView = (ContentView)sender;
            WindowWidth = contentView.Width;
            WindowHeight = contentView.Height;

            if (WindowWidth <= 0 || WindowHeight <= 0)
                return;

            // Orient StackLayout based on portrait/landscape mode.
            stackLayout.Orientation = (WindowWidth < WindowHeight) ? StackOrientation.Vertical :
                                                         StackOrientation.Horizontal;

            // Calculate tile size and position based on ContentView size.
            tileSize = Math.Min(WindowWidth, WindowHeight) / Math.Min(Rows, Cols);
            absoluteLayout.WidthRequest = Cols * tileSize;
            absoluteLayout.HeightRequest = Rows * tileSize;

            foreach (View view in absoluteLayout.Children)
            {
                PhotoPuzzleElement tile = (PhotoPuzzleElement)view;
                tile.InvalidSurfaceState();
                // Set tile bounds.
                AbsoluteLayout.SetLayoutBounds(tile,
                    new Rectangle(tile.Col * tileSize, tile.Row * tileSize, tileSize, tileSize));
            }
        }

        private async void btn1_Clicked(object sender, EventArgs e)
        {
            Assembly asm = GetType().GetTypeInfo().Assembly;
            Stream stream = asm.GetManifestResourceStream(pathToImages + "Pair.Covers.violet_full.png");
            SKBitmap nbsk = SKBitmap.Decode(stream);
            stream.Close();
            for (int i = 0; i < absoluteLayout.Children.Count; i++)
            {
                PhotoPuzzleElement nt = absoluteLayout.Children[i] as PhotoPuzzleElement;

                nt.bitmap1 = nbsk;
                nt.InvalidSurfaceState();
                await Task.Delay(50);
            }
        }
    }
}