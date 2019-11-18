using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
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

        private void InitDrawHelpers()
        {
            OutCircle = new SKPaint { Color = SKColors.Blue };
            InnerCircle = new SKPaint { Color = SKColors.Yellow };
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
    }
}
