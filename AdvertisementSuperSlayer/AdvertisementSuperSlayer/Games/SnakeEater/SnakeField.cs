using AdvertisementSuperSlayer.DbModels;
using AdvertisementSuperSlayer.Helpers;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    partial class SnakeField : SKCanvasView, ISaveResult
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public int SquareWidth { get; set; }
        public CellInfo[][] cellInfos;



        private List<Tuple<int, int>> SnAllBody;
        private SnakeDirection SnDirection;
        private SKPaint Thick;
        private SKPaint EmptyCellPaint;
        private SKPaint OutCircle;
        private SKPaint InnerCircle;
        private Random rnd;
        private Assembly asm;
        private SKBitmap[][] AllAdvBitmaps;
        private SKBitmap[] Head;
        private Stopwatch Timer;


        private double ActualWidth;
        private double ActualHeight;
        private float dydx = 0;
        private bool _TimerState = true;
        private bool ImagesReady = false;
        private float[] _rows_;
        private float[] _cols_;
        private int advRows;
        private int advCols;
        private int BlockCount = 0;
        

        public SnakeField(int rows, int cols)
        {
            asm = GetType().GetTypeInfo().Assembly;
            SizeChanged += OnSnakeFieldSizeChanged;
            advRows = 3;
            advCols = advRows * 3;
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

            SnDirection = SnakeDirection.Right;
            rnd = new Random(DateTime.Now.Millisecond);
            cellInfos = new CellInfo[Rows - 2][];
            for (int i = 0; i < Rows - 2; i++)
                cellInfos[i] = new CellInfo[Cols - 2];
            EmptyCellPaint = new SKPaint { Color = SKColors.Bisque };

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

            cellInfos[0][3].State = ElementState.SnakeBody;
            cellInfos[0][2].State = ElementState.SnakeBody;
            cellInfos[0][1].State = ElementState.SnakeBody;
            cellInfos[0][0].State = ElementState.SnakeBody;


            Play();            
        }


        public bool CheckFreeForImage(int row, int col)
        {
            if (cellInfos[row][col].State.Equals(ElementState.Adv))
                return true;
            return false;
        }



        private void DrawSnake(SKCanvas canvas)
        {
            // Draw head
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



        private bool Eat(int x, int y)
        {
            if (cellInfos[x][y].State == ElementState.Adv)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        private bool MakeStep()
        {
            bool cond0 = CheckSelfEat();
            bool cond1 = SnAllBody[0].Item1 < 0;
            bool cond2 = SnAllBody[0].Item1 >= cellInfos[0].Length;
            bool cond3 = SnAllBody[0].Item2 < 0;
            bool cond4 = SnAllBody[0].Item2 >= cellInfos.Length;

            if (cond1 || cond2 || cond3 || cond4 || cond0)
            {
                Pause();
                SaveResult();
            }
            int f, s;
            switch (SnDirection)
            {
                case SnakeDirection.Up:
                    {
                        Tuple<int, int> pt = new Tuple<int, int>(SnAllBody[0].Item1, SnAllBody[0].Item2 - 1);
                        SnAllBody.Insert(0, pt);
                        break;
                    }
                case SnakeDirection.Down:
                    {
                        Tuple<int, int> pt = new Tuple<int, int>(SnAllBody[0].Item1, SnAllBody[0].Item2 + 1);
                        SnAllBody.Insert(0, pt);
                        break;
                    }
                case SnakeDirection.Left:
                    {
                        Tuple<int, int> pt = new Tuple<int, int>(SnAllBody[0].Item1 - 1, SnAllBody[0].Item2);
                        SnAllBody.Insert(0, pt);
                        break;
                    }
                case SnakeDirection.Right:
                    {
                        Tuple<int, int> pt = new Tuple<int, int>(SnAllBody[0].Item1 + 1, SnAllBody[0].Item2);
                        SnAllBody.Insert(0, pt);
                        break;
                    }
                default:
                    break;
            }
            f = SnAllBody[0].Item2;
            s = SnAllBody[0].Item1;
            
           
            if (!Eat(f, s))
            {
                f = SnAllBody[SnAllBody.Count - 1].Item2;
                s = SnAllBody[SnAllBody.Count - 1].Item1;
                cellInfos[f][s].State = ElementState.Free;
                SnAllBody.RemoveAt(SnAllBody.Count - 1);
                BlockCount--;
            }

            BlockCount++;
            f = SnAllBody[0].Item2;
            s = SnAllBody[0].Item1;
            cellInfos[f][s].State = ElementState.SnakeHead;

            f = SnAllBody[1].Item2;
            s = SnAllBody[1].Item1;
            cellInfos[f][s].State = ElementState.SnakeBody;
            SpawnNew();
            return true;
        }


        private void SpawnNew()
        {
            // There is no AD
            bool check = false;
            for (int i = 0; i < cellInfos.Length; i++)
            {
                for (int j = 0; j < cellInfos[i].Length; j++)
                {
                    if (cellInfos[i][j].State == ElementState.Adv)
                    {
                        check = true;
                        break;
                    }
                }
                if (check)
                    break;
            }

            if (!check)
            {
                SetAdvInfo();
            }
        }



        private void DrawSnakeHead(SKCanvas canvas, int i, int j)
        {
            i++;
            j++;

            switch(SnDirection)
            {
                case SnakeDirection.Left:
                    {
                        canvas.DrawBitmap(Head[0], _cols_[j], _rows_[i]);
                        break;
                    }
                case SnakeDirection.Up:
                    {
                        canvas.DrawBitmap(Head[1], _cols_[j], _rows_[i]);
                        break;
                    }
                case SnakeDirection.Right:
                    {
                        canvas.DrawBitmap(Head[2], _cols_[j], _rows_[i]);
                        break;
                    }
                case SnakeDirection.Down:
                    {
                        canvas.DrawBitmap(Head[3], _cols_[j], _rows_[i]);
                        break;
                    }
            }
        }



        private bool CheckSelfEat()
        {
            for (int i = 1; i < SnAllBody.Count; i++)
            {
                bool cond1 = SnAllBody[0].Item1 == SnAllBody[i].Item1;
                bool cond2 = SnAllBody[0].Item2 == SnAllBody[i].Item2;

                if (cond1 && cond2)
                    return true;
            }
            return false;
        }

        public void SaveResult()
        {
            Timer.Stop();
            long mills = Timer.ElapsedMilliseconds;
            SnakeRecord record = new SnakeRecord
            {
                GameTime =  TimeSpan.FromMilliseconds(mills),
                LastModified = DateTime.UtcNow,
                MaxScore = BlockCount
            };
            App.Rest.UpdateSnake(record);
            //Navigation.
        }
    }
}
