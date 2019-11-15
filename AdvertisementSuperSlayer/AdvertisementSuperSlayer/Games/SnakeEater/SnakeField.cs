using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    class SnakeField : SKCanvasView
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public int SquareWidth { get; set; }
        public LinkedList<SnakeAdvBitmap> advs;

        public CellInfo[][] cellInfos;
        private float[] _rows_;
        private float[] _cols_;
        private SKPaint Thick;
        private float dydx = 0;

        public SnakeField(int rows, int cols)
        {
            Cols = cols;
            Rows = rows;
            _rows_ = new float[Rows + 1];
            _cols_ = new float[Cols + 1];
            Thick = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Orange,
                StrokeWidth = 4
            };



            cellInfos = new CellInfo[Rows - 2][];
            for (int i = 0; i < Rows - 2; i++)
                cellInfos[i] = new CellInfo[Cols - 2];

            SizeChanged += OnSnakeFieldSizeChanged;
            PaintSurface += OnCanvasViewPaintSurface;
            InvalidateSurface();
        }

        
        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            DrawGrid(canvas);
        }

        private void DrawBitmaps(SKCanvas canvas)
        {

        }

        public void DrawGrid(SKCanvas canvas)
        {
            
            // Horizontal lines
            for (int i = 0; i < _rows_.Length; i++)
            {
                canvas.DrawLine(0, _rows_[i], _cols_[_cols_.Length - 1], _rows_[i], Thick);
                //canvas.DrawLine(0, _rows_[i], _rows_[_rows_.Length - 1], _rows_[i], Thick);
                //canvas.DrawLine(0, _rows_[i], (float)Width, _rows_[i], Inner);
            }

            // Vertical lines
            for (int i = 0; i < _cols_.Length; i++)
            {
                //canvas.DrawLine(_cols_[i], 0, _cols_[i], (float)Height, Thick);
                canvas.DrawLine(_cols_[i], 0, _cols_[i], _rows_[_rows_.Length - 1], Thick);
            }

            Thick.Style = SKPaintStyle.StrokeAndFill;
            SKRect rect = new SKRect(0, 0, dydx, (float)Height);
            canvas.DrawRect(rect, Thick);
            rect = new SKRect(0, 0, _cols_[_cols_.Length - 1], dydx);
            canvas.DrawRect(rect, Thick);
            rect = new SKRect(_cols_[_cols_.Length - 2], 0, _cols_[_cols_.Length - 1], _rows_[_rows_.Length - 1]);
            canvas.DrawRect(rect, Thick);
            rect = new SKRect(0, _rows_[_rows_.Length - 2], _cols_[_cols_.Length - 1], _rows_[_rows_.Length - 1]);
            canvas.DrawRect(rect, Thick);
            Thick.Style = SKPaintStyle.Stroke;
            Thick.TextSize = 50;
            canvas.DrawText(Width + "  " + Height, 400, 400, Thick);
        }

        private void OnSnakeFieldSizeChanged(object sender, EventArgs args)
        {
            Thick.StrokeWidth = (int)((float)(Width * 0.0017));

            dydx = (float)(Height / Rows);

            for (int i = 0; i < _rows_.Length; i++)
            {
                _rows_[i] = i * dydx;
            }
            for (int i = 0; i < _cols_.Length; i++)
            {
                _cols_[i] = i * dydx;
            }
            SquareWidth = (int)Math.Round(dydx - Thick.StrokeWidth / 2.0f);
            InvalidateSurface();
        }
    }
}
