using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    partial class SnakeField : SKCanvasView
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public int SquareWidth { get; set; }
        public List<SnakeAdvBitmap> advs;


        public CellInfo[][] cellInfos;

        private List<Tuple<int, int>> SnAllBody;
        private float[] _rows_;
        private float[] _cols_;
        private SKPaint Thick;
        private SKPaint EmptyCellPaint;
        private SKPaint OutCircle;
        private SKPaint InnerCircle;
        private SnakeDirection SnDirection;
        private double ActualWidth;
        private double ActualHeight;
        private float dydx = 0;
        private bool _TimerState = true;
        private ImageGroupManager ImgManager;
        private Random rnd;

        public SnakeField(int rows, int cols)
        {
            Cols = cols + 2;
            Rows = rows + 2;
            _rows_ = new float[Rows + 1];
            _cols_ = new float[Cols + 1];
            Thick = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Orange,
                StrokeWidth = 4
            };
            advs = new List<SnakeAdvBitmap>();
            ImgManager = new ImageGroupManager(2, 3, SquareWidth);

            SnDirection = SnakeDirection.Right;
            rnd = new Random(DateTime.Now.Millisecond);

            cellInfos = new CellInfo[Rows - 2][];
            for (int i = 0; i < Rows - 2; i++)
                cellInfos[i] = new CellInfo[Cols - 2];


            EmptyCellPaint = new SKPaint { Color = SKColors.Bisque };

            SizeChanged += OnSnakeFieldSizeChanged;
            PaintSurface += OnCanvasViewPaintSurface;
            InitDrawHelpers();
            InitGestures();
            InitCellInfos();


            SnAllBody = new List<Tuple<int, int>>()
            {
                new Tuple<int, int>(3, 0),
                new Tuple<int, int>(2, 0),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(0, 0)
            };
            Play();
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

            DrawSnake(canvas);
        }

        public void DrawBitmaps(SKCanvas canvas)
        {
            foreach(SnakeAdvBitmap skBitmap in advs)
            {
                if (skBitmap.WasEaten)
                {
                    continue;
                }

                // check here if place is not busy
                canvas.DrawBitmap(skBitmap, _rows_[skBitmap.RowIndex], _cols_[skBitmap.ColIndex]);
            }
        }

        public bool CheckFreeForImage(int row, int col)
        {
            if (cellInfos[row][col].State.Equals(ElementState.Adv))
                return true;
            return false;
        }

        private void DrawSnake(SKCanvas canvas)
        {
            for (int i = 0; i < SnAllBody.Count; i++)
            {
                DrawSnakeBodyElement(canvas, SnAllBody[i].Item2, SnAllBody[i].Item1);
            }
        }
        private void DrawSnakeBodyElement(SKCanvas canvas, int row, int col)
        {
            row++;
            col++;
            float pX = _cols_[col] + SquareWidth / 2.0f;
            float pY = _rows_[row] + SquareWidth / 2.0f;
            float param = 0.45f;
            canvas.DrawCircle(pX, pY, param * SquareWidth, OutCircle);
            canvas.DrawCircle(pX, pY, param * 0.667f * SquareWidth, InnerCircle);
        }

        public void DrawGrid(SKCanvas canvas)
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
            Thick.TextSize = 50;
            canvas.DrawText(ActualWidth + "  " + ActualHeight, 400, 400, Thick);
        }


        private void Eat()
        {
            foreach(SnakeAdvBitmap bmp in advs)
            {
                if (bmp.RowIndex == SnAllBody[0].Item2 && bmp.ColIndex == SnAllBody[0].Item1)
                {
                    bmp.WasEaten = true;
                }
            }
        }

        public void CheckGroups()
        {
            LinkedList<int> toRemove = new LinkedList<int>();
            int count;
            // Find images wich should be removes
            foreach(int[] tmpArr in ImgManager.Collection)
            {
                count = 0;
                foreach(int _img in tmpArr)
                {
                    if (advs[_img].WasEaten)
                        count++;
                }
                if (count == ImgManager.ImageCols * ImgManager.ImageRows)
                {
                    toRemove.AddLast(tmpArr[0]);
                }
            }

            // Delete all SUBImages from collection
            foreach (int rem in toRemove)
            {
                // ImgManager.Remove(rem);
                
                for (int i = rem; i < rem + ImgManager.ImageCols * ImgManager.ImageRows; i++)
                {
                    int toDel = advs.FindIndex(pred => pred.OrderNumber == rem);
                    advs.RemoveAt(toDel);
                }
            }
        }

        private bool MakeStep()
        {
            int s = SnAllBody[SnAllBody.Count - 1].Item1;
            int f = SnAllBody[SnAllBody.Count - 1].Item2;
            cellInfos[f][s].State = ElementState.Free;

            for (int i = SnAllBody.Count - 1; i > 0; i--)
            {
                SnAllBody[i] = SnAllBody[i - 1];
            }

            switch (SnDirection)
            {
                case SnakeDirection.Up:
                    {
                        Tuple<int, int> pt = new Tuple<int, int>(SnAllBody[0].Item1, SnAllBody[0].Item2 - 1);
                        SnAllBody[0] = pt;
                        break;
                    }
                case SnakeDirection.Down:
                    {
                        Tuple<int, int> pt = new Tuple<int, int>(SnAllBody[0].Item1, SnAllBody[0].Item2 + 1);
                        SnAllBody[0] = pt;
                        break;
                    }
                case SnakeDirection.Left:
                    {
                        Tuple<int, int> pt = new Tuple<int, int>(SnAllBody[0].Item1 - 1, SnAllBody[0].Item2);
                        SnAllBody[0] = pt;
                        break;
                    }
                case SnakeDirection.Right:
                    {
                        Tuple<int, int> pt = new Tuple<int, int>(SnAllBody[0].Item1 + 1, SnAllBody[0].Item2);
                        SnAllBody[0] = pt;
                        break;
                    }
                default:
                    break;
            }

            bool cond1 = SnAllBody[0].Item1 < 0;
            bool cond2 = SnAllBody[0].Item1 >= cellInfos.Length;
            bool cond3 = SnAllBody[0].Item2 < 0;
            bool cond4 = SnAllBody[0].Item2 >= cellInfos[0].Length;

            if (cond1 || cond2 || cond3 || cond4)
            {

            }

            Eat();







            s = SnAllBody[0].Item1;
            f = SnAllBody[0].Item2;
            cellInfos[f][s].State = ElementState.SnakeHead;

            // Check if it ate something
            return true;
        }

        private bool CheckSelfEat()
        {
            for (int i = 4; i < SnAllBody.Count; i++)
            {
                bool cond1 = SnAllBody[0].Item1 == SnAllBody[i].Item1;
                bool cond2 = SnAllBody[0].Item2 == SnAllBody[i].Item2;

                // If it's not
                if (cond1 && cond2)
                    return false;
            }
            return true;
        }


        private void DrawAdv(SKCanvas canvas)
        {
            // Problem MB here!!!!!!!!!!!!!!!!!!!!!!
            foreach(SnakeAdvBitmap sadv in advs)
            {
                if (!sadv.WasEaten)
                {
                    int row = sadv.RowIndex + 1;
                    int col = sadv.ColIndex + 1;
                    canvas.DrawBitmap(sadv, _cols_[col], _rows_[row]);
                }
            }
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
            InvalidateSurface();
        }

        private Tuple<int, int> GetAdvCoord()
        {
            LinkedList<Tuple<int, int>> free = new LinkedList<Tuple<int, int>>();
            for (int r = 0; r < cellInfos.Length - ImgManager.ImageRows; r++)
            {
                for (int c = 0; c < cellInfos[0].Length - ImgManager.ImageCols; c++)
                {
                    bool skipState = false;
                    for (int i = 0; i < ImgManager.ImageRows; i++)
                    {
                        for (int j = 0; j < ImgManager.ImageCols; j++)
                        {

                            if (!(cellInfos[r + i][c + j].State == ElementState.Free))
                            {
                                skipState = true;
                                break;
                            }
                        }
                        if (skipState)
                            break;
                    }
                    if (!skipState)
                        free.AddLast(new Tuple<int, int>(r, c));
                }
            }
            int index = rnd.Next(free.Count);
            return free.ElementAt(index);
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
