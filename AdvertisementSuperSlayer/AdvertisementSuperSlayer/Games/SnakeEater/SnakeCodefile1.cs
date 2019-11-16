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
                for (int j = 0; j < cellInfos.GetLength(1); j++)
                {
                    cellInfos[i][j] = new CellInfo { State = ElementState.Free };
                }
            }

            for (int i = 1; i < _rows_.Length - 1; i++)
            {
                for (int j = 1; j < _cols_.Length; j++)
                {
                    cellInfos[i - 1][j - 1].RowIndex = i;
                    cellInfos[i - 1][j - 1].ColIndex = j;
                }
            }

        }

        private void InitGestures()
        {
            SwipeGestureRecognizer swpUp = new SwipeGestureRecognizer();
            swpUp.Direction = SwipeDirection.Up;
            swpUp.Swiped += (s, e) => { SnDirection = SnakeDirection.Up; };

            SwipeGestureRecognizer swpDown = new SwipeGestureRecognizer();
            swpDown.Direction = SwipeDirection.Down;
            swpDown.Swiped += (s, e) => { SnDirection = SnakeDirection.Down; };

            SwipeGestureRecognizer swpLeft = new SwipeGestureRecognizer();
            swpLeft.Direction = SwipeDirection.Left;
            swpLeft.Swiped += (s, e) => { SnDirection = SnakeDirection.Left; };

            SwipeGestureRecognizer swpRight = new SwipeGestureRecognizer();
            swpRight.Direction = SwipeDirection.Right;
            swpRight.Swiped += (s, e) => { SnDirection = SnakeDirection.Right; };

            
            GestureRecognizers.Add(swpUp);
            GestureRecognizers.Add(swpDown);
            GestureRecognizers.Add(swpLeft);
            GestureRecognizers.Add(swpRight);
        }
    }
}
