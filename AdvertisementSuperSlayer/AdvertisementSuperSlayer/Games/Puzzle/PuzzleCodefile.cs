using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using SkiaSharp;

namespace AdvertisementSuperSlayer.Games.Puzzle
{
    partial class PuzzlePage : ContentPage
    {
        private void HandleImage(string pathToImage)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(pathToImage);
            SKBitmap fullBitmap = SKBitmap.Decode(stream);
            stream.Close();

            int width = fullBitmap.Width / Cols;
            int height = fullBitmap.Height / Rows;

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    PhotoPuzzleElement bmp = new PhotoPuzzleElement();
                    bmp.bitmap1 = new SKBitmap(width, height);
                    SKRect dest = new SKRect(0, 0, width, height);
                    SKRect source =
                        new SKRect(col * width, row * height, (col + 1) * width, (row + 1) * height);
                   
                    using(SKCanvas canvas = new SKCanvas(bmp.bitmap1))
                    {
                        canvas.DrawBitmap(fullBitmap, source, dest);
                    }
                    PuzzleElements[row][col] = bmp;
                    PuzzleElements[row][col].Row = row;
                    PuzzleElements[row][col].Col = col;
                    absoluteLayout.Children.Add(PuzzleElements[row][col]);
                }
            }
        }
    }
}
