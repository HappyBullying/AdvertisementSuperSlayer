using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    partial class SnakeField : SKCanvasView
    {
        private void InitCellInfos()
        {
            cellInfos = new CellInfo[Rows - 2][];
            for (int i = 0; i < cellInfos.GetLength(0); i++)
            {
                cellInfos[i] = new CellInfo[Cols - 2];
                for (int j = 0; j < Cols - 2; j++)
                {
                    cellInfos[i][j] = new CellInfo { State = ElementState.Free };
                }
            }

            for (int i = 1; i < _rows_.Length - 2; i++)
            {
                for (int j = 1; j < _cols_.Length - 2; j++)
                {
                    cellInfos[i - 1][j - 1].RowIndex = i;
                    cellInfos[i - 1][j - 1].ColIndex = j;
                }
            }

        }


        private void DrawGrid(SKCanvas canvas)
        {

            // Horizontal lines
            for (int i = 0; i < _rows_.Length; i++)
            {
                canvas.DrawLine(0, _rows_[i], _cols_[_cols_.Length - 1], _rows_[i], Thick);
            }

            // Vertical lines
            for (int i = 0; i < _cols_.Length; i++)
            {
                canvas.DrawLine(_cols_[i], 0, _cols_[i], _rows_[_rows_.Length - 1], Thick);
            }

            Thick.Style = SKPaintStyle.StrokeAndFill;
            SKRect rect = new SKRect(0, 0, dydx, (float)ActualHeight);
            canvas.DrawRect(rect, Thick);
            rect = new SKRect(0, 0, _cols_[_cols_.Length - 1], dydx);
            canvas.DrawRect(rect, Thick);
            rect = new SKRect(_cols_[_cols_.Length - 2], 0, _cols_[_cols_.Length - 1], _rows_[_rows_.Length - 1]);
            canvas.DrawRect(rect, Thick);
            rect = new SKRect(0, _rows_[_rows_.Length - 2], _cols_[_cols_.Length - 1], _rows_[_rows_.Length - 1]);
            canvas.DrawRect(rect, Thick);
            Thick.Style = SKPaintStyle.Stroke;

        }

        private void InitDrawHelpers()
        {
            OutCircle = new SKPaint 
            {
                Color = SKColors.Blue,
                IsAntialias = true
            };
            InnerCircle = new SKPaint
            {
                Color = SKColors.Yellow,
                IsAntialias = true
            };
        }

        private void InitGestures()
        {
            SwipeGestureRecognizer swpUp = new SwipeGestureRecognizer();
            swpUp.Direction = SwipeDirection.Up;
            swpUp.Swiped += (s, e) => 
            { 
                if (SnAllBody[0].Item1 != SnAllBody[1].Item1)
                    SnDirection = SnakeDirection.Up; 
            };

            SwipeGestureRecognizer swpDown = new SwipeGestureRecognizer();
            swpDown.Direction = SwipeDirection.Down;
            swpDown.Swiped += (s, e) => 
            {
                if (SnAllBody[0].Item1 != SnAllBody[1].Item1)
                    SnDirection = SnakeDirection.Down; 
            };

            SwipeGestureRecognizer swpLeft = new SwipeGestureRecognizer();
            swpLeft.Direction = SwipeDirection.Left;
            swpLeft.Swiped += (s, e) => 
            { 
                if (SnAllBody[0].Item2 != SnAllBody[1].Item2)
                    SnDirection = SnakeDirection.Left; 
            };

            SwipeGestureRecognizer swpRight = new SwipeGestureRecognizer();
            swpRight.Direction = SwipeDirection.Right;
            swpRight.Swiped += (s, e) => 
            {
                if (SnAllBody[0].Item2 != SnAllBody[1].Item2)
                    SnDirection = SnakeDirection.Right; 
            };

            
            GestureRecognizers.Add(swpUp);
            GestureRecognizers.Add(swpDown);
            GestureRecognizers.Add(swpLeft);
            GestureRecognizers.Add(swpRight);
        }

        private void OnSnakeFieldSizeChanged(object sender, EventArgs args)
        {
            Xamarin.Essentials.DisplayInfo DInfo = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo;
            double scale = DInfo.Width / Width;
            ActualHeight = Height * scale;
            ActualWidth = Width * scale;

            Thick.StrokeWidth = (int)((float)(ActualWidth * 0.0017));

            dydx = (float)(ActualHeight / Rows);

            for (int i = 0; i < _rows_.Length; i++)
            {
                _rows_[i] = i * dydx;
            }
            for (int i = 0; i < _cols_.Length; i++)
            {
                _cols_[i] = i * dydx;
            }

            SquareWidth = (int)Math.Round(dydx);

            if (!ImagesReady)
            {
                InitAdvImages();
                SetAdvInfo();
                ImagesReady = true;
            }

            InvalidateSurface();
        }




        /// <summary>
        /// Draws all cells according to their cellinfo
        /// </summary>
        private void DrawAll(SKCanvas canvas)
        {
            for (int i = 0; i < cellInfos.Length; i++)
            {
                for (int j = 0; j < cellInfos[i].Length; j++)
                {
                    switch(cellInfos[i][j].State)
                    {
                        case ElementState.SnakeBody:
                            {
                                DrawSnakeBodyElement(canvas, i, j);
                                break;
                            }
                        case ElementState.Adv:
                            {
                                canvas.DrawBitmap(cellInfos[i][j].Bmp, _cols_[j + 1], _rows_[i + 1]);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
        }



        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            SKRect rect = new SKRect(_cols_[1], _rows_[1], _cols_[_cols_.Length - 2], _rows_[_rows_.Length - 2]);
            canvas.DrawRect(rect, EmptyCellPaint);

            DrawGrid(canvas);

            //DrawSnake(canvas);
            DrawAll(canvas);

        }


        private void SetAdvInfo()
        {
            int bmpId = rnd.Next(AllAdvBitmaps.Length);

            Tuple<int, int> tmp = GetPointForImage();

            for (int row = 0; row < advRows; row++)
            {
                for (int col = 0; col < advCols; col++)
                {
                    cellInfos[row + tmp.Item1][col + tmp.Item2].Bmp = AllAdvBitmaps[bmpId][row * advCols + col];
                    cellInfos[row + tmp.Item1][col + tmp.Item2].State = ElementState.Adv;
                }
            }
        }




        public void Play()
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(200);
            Device.StartTimer(ts, () =>
            {
                MakeStep();
                InvalidateSurface();
                return _TimerState;
            });
        }

        public void Pause()
        {
            _TimerState = false;
        }
    }
}
