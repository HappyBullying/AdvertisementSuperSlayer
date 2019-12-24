using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AdvertisementSuperSlayer.Helpers;
using AdvertisementSuperSlayer.DbModels;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.Pair
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PairCardsPage : ContentPage, ISaveResult
    {
        public PairCardsPage(int row, int col)
        {
            InitializeComponent();
            Rows = row;
            Cols = col;
            tiles = new PhotoHalfPairTile[row][];
            _Busy = new BusyBehavior();
            RightConfiguration += OnRightConfiguration;
            WrongConfiguration += OnWrongConfiguration;
            BackG.Source = ImageSource.FromResource(App.PathToImages + "Pair.pair_background.png");
            Init();
        }


        private void Init()
        {
            ImInfo = new SKImageInfo(150, 150);
            rnd = new Random(DateTime.Now.Millisecond);
            List<string> faceImages = new List<string>();
            string[] folders = new string[] { "lightblue", "lightgreen", "violet" };
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            string selected_color = "";
            InitSound();
            InitCover(assembly, out selected_color);
            InitCheckMark(assembly, selected_color);

            string frontFolder = "";
            if (selected_color == "blue")
                frontFolder = "lightblue";
            if (selected_color == "green")
                frontFolder = "lightgreen";
            if (selected_color == "violet")
                frontFolder = "violet";

            IEnumerable<string> faces = from _tmp_faces_ in assembly.GetManifestResourceNames()
                                        where _tmp_faces_.Contains(frontFolder) && _tmp_faces_.Contains("Faces")
                                        select _tmp_faces_;
            for (int i = 0; i < Rows * Cols / 2; i++)
            {
                faceImages.Add(faces.ElementAt(i));
                faceImages.Add(faces.ElementAt(i));
            }


            Stream frontStream = null;
            for (int i = 0; i < Rows; i++)
            {
                tiles[i] = new PhotoHalfPairTile[Cols];
                for (int j = 0; j < Cols; j++)
                {
                    int elem = rnd.Next(faceImages.Count);
                    string name = faceImages[elem];
                    faceImages.RemoveAt(elem);
                    string fullPath = name;

                    frontStream = assembly.GetManifestResourceStream(fullPath);
                    SKBitmap front = SKBitmap.Decode(frontStream).Resize(ImInfo, SKFilterQuality.High);
                    tiles[i][j] = new PhotoHalfPairTile(TileBitmap, front);
                    tiles[i][j].Row = i;
                    tiles[i][j].Col = j;

                    name = Path.GetFileNameWithoutExtension(name);
                    int lInd = name.LastIndexOf('.');
                    name = name.Substring(lInd + 1);

                    tiles[i][j].FrontBitmapName = name;
                    TapGestureRecognizer tgr = new TapGestureRecognizer();
                    tgr.CommandParameter = tiles[i][j];
                    tgr.Command = new Command<PhotoHalfPairTile>(ExecuteRotation, CanExecuteRotation);
                    tiles[i][j].AddGestureRecognizer(tgr);
                    absoluteLayout.Children.Add(tiles[i][j]);
                    frontStream.Close();
                }
            }
        }

        private void InitCheckMark(Assembly asm, string color)
        {
            string pathTochckM = App.PathToImages + "Pair.CheckMarks.check_mark_";
            switch (color)
            {
                case "violet":
                    {
                        pathTochckM += "violet.png";
                        Stream chckMStream = asm.GetManifestResourceStream(pathTochckM);
                        CheckMark = SKBitmap.Decode(chckMStream);
                        chckMStream.Close();
                        break;
                    }
                case "blue":
                    {
                        pathTochckM += "blue.png";
                        Stream chckMStream = asm.GetManifestResourceStream(pathTochckM);
                        CheckMark = SKBitmap.Decode(chckMStream);
                        chckMStream.Close();
                        break;
                    }
                case "green":
                    {
                        pathTochckM += "green.png";
                        Stream chckMStream = asm.GetManifestResourceStream(pathTochckM);
                        CheckMark = SKBitmap.Decode(chckMStream);
                        chckMStream.Close();
                        break;
                    }
                default:
                    throw new Exception();
            }
            CheckMark = CheckMark.Resize(ImInfo, SKFilterQuality.High);
        }

        private void InitCover(Assembly asm, out string color)
        {
            color = "";
            IEnumerable<string> covers = from _tmp_ in asm.GetManifestResourceNames()
                                         where _tmp_.Contains("Covers")
                                         select _tmp_;
            int color_item_val = rnd.Next(covers.Count());
            string selected_bmp = covers.ElementAt(color_item_val);

            if (selected_bmp.Contains("green"))
                color = "green";
            if (selected_bmp.Contains("violet"))
                color = "violet";
            if (selected_bmp.Contains("blue"))
                color = "blue";

            Stream coverStream = asm.GetManifestResourceStream(selected_bmp);
            TileBitmap = SKBitmap.Decode(coverStream).Resize(ImInfo, SKFilterQuality.High);
            coverStream.Close();
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
                PhotoHalfPairTile tile = (PhotoHalfPairTile)view;
                tile.InvalidSurfaceState();
                // Set tile bounds.
                AbsoluteLayout.SetLayoutBounds(tile,
                    new Rectangle(tile.Col * tileSize, tile.Row * tileSize, tileSize, tileSize));
            }
        }


        private async void InitSound()
        {
            await Task.Run(() =>
            {
                wins = new IAudio[3];
                defeats = new IAudio[3];

                wins[0] = DependencyService.Get<IAudio>(DependencyFetchTarget.NewInstance);
                wins[1] = DependencyService.Get<IAudio>(DependencyFetchTarget.NewInstance);
                wins[2] = DependencyService.Get<IAudio>(DependencyFetchTarget.NewInstance);

                defeats[0] = DependencyService.Get<IAudio>(DependencyFetchTarget.NewInstance);
                defeats[1] = DependencyService.Get<IAudio>(DependencyFetchTarget.NewInstance);
                defeats[2] = DependencyService.Get<IAudio>(DependencyFetchTarget.NewInstance);


                wins[0].SetupAudioFile("win1.mp3", false, 1);
                wins[1].SetupAudioFile("win2.mp3", false, 1);
                wins[2].SetupAudioFile("win3.mp3", false, 1);

                defeats[0].SetupAudioFile("lose1.mp3", false, 1);
                defeats[1].SetupAudioFile("lose2.mp3", false, 1);
                defeats[2].SetupAudioFile("lose3.mp3", false, 1);
            });
        }

        public void SaveResult()
        {
            Timer.Stop();
            long mills = Timer.ElapsedMilliseconds;
            PairRecord record = new PairRecord
            {
                Errors = ErrorCounter,
                GameDuration = TimeSpan.FromMilliseconds(mills),
                LasModified = DateTime.UtcNow
            };

            App.Rest.UpdatePair(record);
        }
    }
}