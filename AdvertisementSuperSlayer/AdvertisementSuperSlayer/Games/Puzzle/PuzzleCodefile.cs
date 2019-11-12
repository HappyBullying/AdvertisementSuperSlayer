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
        private List<SKBitmap> HandleImage(string pathToImage)
        {
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(pathToImage);
            SKBitmap fullBitmap = SKBitmap.Decode(stream);
            stream.Close();

            int width = fullBitmap.Width / Cols;
            int height = fullBitmap.Height / Rows;
            List<SKBitmap> ret = new List<SKBitmap>();

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    PuzzleElements[row][col] = new PhotoPuzzleElement();
                    PuzzleElements[row][col].PuzzleId = "-1_-1";
                    right_combo[row][col] = $"{row}_{col}";
                    SKRect dest = new SKRect(0, 0, width, height);
                    SKRect source =
                        new SKRect(col * width, row * height, (col + 1) * width, (row + 1) * height);
                    SKBitmap tmpBmtp = new SKBitmap(width, height);
                    using(SKCanvas canvas = new SKCanvas(tmpBmtp))
                    {
                        canvas.DrawBitmap(fullBitmap, source, dest);
                    }
                    ret.Add(tmpBmtp);
                    PuzzleElements[row][col].Row = row;
                    PuzzleElements[row][col].Col = col;
                }
            }
            return ret;
        }

        private void InitPuzzle(List<SKBitmap> bmps)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    int randomVal = rnd.Next(bmps.Count);
                    PuzzleElements[row][col].bitmap1 = bmps[randomVal];
                    PuzzleElements[row][col].bitmap2 = null;
                    PuzzleElements[row][col].PuzzleId = $"{row}_{col}";
                    absoluteLayout.Children.Add(PuzzleElements[row][col]);
                    bmps.RemoveAt(randomVal);
                }
            }
        }
    }
}
