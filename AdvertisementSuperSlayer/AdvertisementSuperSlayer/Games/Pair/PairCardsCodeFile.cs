using SkiaSharp;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using AdvertisementSuperSlayer.Helpers;

namespace AdvertisementSuperSlayer.Games.Pair
{
    public partial class PairCardsPage : ContentPage
    {
        private double WindowWidth { get; set; }
        private double WindowHeight { get; set; }
        private int Rows, Cols;
        private SKBitmap TileBitmap;
        private SKBitmap CheckMark;
        private BusyBehavior _Busy;
        private double tileSize;
        private PhotoHalfPairTile[][] tiles;
        private readonly string PathToImages = "AdvertisementSuperSlayer.Images.Pair.";
        private event EventHandler<ConfigEventArgs> RightConfiguration;
        private event EventHandler<ConfigEventArgs> WrongConfiguration;
        private SKImageInfo ImInfo;
        private IAudio[] wins;
        private IAudio[] defeats;
        private Random rnd;

        private async void ExecuteRotation(PhotoHalfPairTile tmp)
        {
            const float degreeStep = 15.0f;
            const int delayMicrosecond = 15;
            _Busy.Take(tmp);
            tmp.IsRotating = true;
            tmp.Flipped = true;
            await Task.Run(async () =>
            {
                while (tmp.Deg <= 90 - degreeStep)
                {
                    await Task.Delay(delayMicrosecond);
                    tmp.Deg = tmp.Deg + degreeStep;
                }
                while (tmp.Deg >= 0 + degreeStep)
                {
                    await Task.Delay(delayMicrosecond);
                    tmp.Deg = tmp.Deg - degreeStep;
                }
                tmp.Deg = 0;
                tmp.IsRotating = false;
            });


            if (_Busy.State.Equals(BusyStates.Filled) || _Busy.State.Equals(BusyStates.Right))
                await Task.Run(() =>
                {
                    int SRow = _Busy.Second.Row;
                    int SCol = _Busy.Second.Col;
                    while (tmp.IsRotating || tiles[SRow][SCol].IsRotating)
                    {
                        continue;
                    }
                });
            if (_Busy.State.Equals(BusyStates.Right))
            {
                ConfigEventArgs args = new ConfigEventArgs
                {
                    FRow = _Busy.First.Row,
                    FCol = _Busy.First.Col,
                    SRow = _Busy.Second.Row,
                    SCol = _Busy.Second.Col
                };
                RightConfiguration?.Invoke(this, args);
            }
            if (_Busy.State.Equals(BusyStates.Filled))
            {
                ConfigEventArgs args = new ConfigEventArgs
                {
                    FRow = _Busy.First.Row,
                    FCol = _Busy.First.Col,
                    SRow = _Busy.Second.Row,
                    SCol = _Busy.Second.Col
                };
                WrongConfiguration?.Invoke(this, args);
            }
        }
        private bool CanExecuteRotation(object sender)
        {
            bool cond1 = _Busy.State.Equals(BusyStates.AllFree);
            bool cond2 = _Busy.State.Equals(BusyStates.OneFree);
            bool cond3 = (sender as PhotoHalfPairTile).Flipped;

            if ((cond1 || cond2) && !cond3)
                return true;
            return false;
        }

        private async void OnRightConfiguration(object sender, ConfigEventArgs args)
        {
            int FRow = args.FRow;
            int FCol = args.FCol;
            int SRow = args.SRow;
            int SCol = args.SCol;
            bool cond1 = tiles[FRow][FCol].IsRotating;
            bool cond2 = tiles[SRow][SCol].IsRotating;
            while (cond1 || cond2)
            {
                continue;
            }
            int soundId = rnd.Next(wins.Length);
            await wins[soundId].PlaySoundAsync();
            await Task.Delay(200);
            tiles[FRow][FCol].GestureRecognizers.Clear();
            tiles[SRow][SCol].GestureRecognizers.Clear();
            tiles[FRow][FCol].SetOk(CheckMark);
            tiles[SRow][SCol].SetOk(CheckMark);
            _Busy.Release();
        }

        private async void OnWrongConfiguration(object sender, ConfigEventArgs args)
        {
            const float degreeStep = 15.0f;
            const int delayMicrosecond = 15;
            int FRow = args.FRow;
            int FCol = args.FCol;
            int SRow = args.SRow;
            int SCol = args.SCol;
            int soundId = rnd.Next(defeats.Length);
            await defeats[soundId].PlaySoundAsync();
            await Task.Run(async () =>
            {
                tiles[FRow][FCol].IsRotating = true;
                tiles[SRow][SCol].IsRotating = true;
                await Task.Delay(1000);
                while (tiles[FRow][FCol].Deg <= 90 - degreeStep)
                {
                    await Task.Delay(delayMicrosecond);
                    tiles[FRow][FCol].Deg = tiles[FRow][FCol].Deg + degreeStep;
                    tiles[SRow][SCol].Deg = tiles[SRow][SCol].Deg + degreeStep;
                }
                while (tiles[FRow][FCol].Deg >= 0 + degreeStep)
                {
                    await Task.Delay(delayMicrosecond);
                    tiles[FRow][FCol].Deg = tiles[FRow][FCol].Deg - degreeStep;
                    tiles[SRow][SCol].Deg = tiles[SRow][SCol].Deg - degreeStep;
                }
                tiles[FRow][FCol].Deg = 0;
                tiles[SRow][SCol].Deg = 0;
                tiles[FRow][FCol].IsRotating = false;
                tiles[SRow][SCol].IsRotating = false;
            });
            ////
            _Busy.Release();
            bool cond1 = tiles[FRow][FCol].WasTapped;
            bool cond2 = tiles[SRow][SCol].WasTapped;
            if (cond1 || cond2)
                await Navigation.PushAsync(new Browser.BrowserPage());
            tiles[FRow][FCol].WasTapped = true;
            tiles[SRow][SCol].WasTapped = true;

            tiles[FRow][FCol].Flipped = false;
            tiles[SRow][SCol].Flipped = false;
        }
    }
}
