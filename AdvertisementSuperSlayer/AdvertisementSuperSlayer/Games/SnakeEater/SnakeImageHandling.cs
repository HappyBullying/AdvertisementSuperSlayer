using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    partial class SnakeField : SKCanvasView
    {

        private Tuple<int, int> GetPointForImage()
        {
            List<Tuple<int, int>> freeCells = new List<Tuple<int, int>>();


            int i, j, m, n;
            bool _break = false;
            for (i = 0; i < cellInfos.Length - advRows; i++)
            {
                for (j = 0; j < cellInfos[0].Length - advCols; j++)
                {
                    _break = false;
                    for (m = 0; m < advRows; m++)
                    {
                        for (n = 0; n < advCols; n++)
                        {
                            if (cellInfos[i + m][j + n].State != ElementState.Free)
                            {
                                _break = true;
                                break;
                            }
                        }
                        if (_break)
                            break;
                    }
                    if (!_break)
                    {
                        freeCells.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return freeCells[rnd.Next(freeCells.Count)];
        }


        private SKBitmap[] SplitBitmap(string fileName)
        {
            SKImageInfo inf = new SKImageInfo(advCols * SquareWidth, advRows * SquareWidth);
            asm = GetType().GetTypeInfo().Assembly;
            Stream stream = asm.GetManifestResourceStream(fileName);
            SKBitmap fullBitmap = SKBitmap.Decode(stream);
            stream.Close();
            SKBitmap[] skBitmaps = new SKBitmap[advCols * advRows];

            int scl = fullBitmap.Height / advRows;
            for (int row = 0; row < advRows; row++)
            {
                for (int col = 0; col < advCols; col++)
                {
                    skBitmaps[row * advCols + col] = new SKBitmap(SquareWidth, SquareWidth);
                    SKRect dest = new SKRect(0, 0, SquareWidth, SquareWidth);
                    SKRect source = new SKRect(col * scl, row * scl, (col + 1) * scl, (row + 1) * scl);

                    using (SKCanvas canvas = new SKCanvas(skBitmaps[row * advCols + col]))
                    {
                        canvas.Clear();
                        canvas.DrawBitmap(fullBitmap, source, dest);
                    }
                }
            }
            return skBitmaps;
        }


        private void InitSnakeHead()
        {
            string path = App.PathToImages + "Snake.Head.";
            string[] images =
            {
                path + "open_left.png",
                path + "open_up.png",
                path + "open_right.png",
                path + "open_down.png"
            };

            Head = new SKBitmap[images.Length];
    
            for (int i = 0; i < images.Length; i++)
            {
                SKImageInfo inf = new SKImageInfo(SquareWidth, SquareWidth);
                using(Stream stream = asm.GetManifestResourceStream(images[i]))
                {
                    Head[i] = SKBitmap.Decode(stream).Resize(inf, SKFilterQuality.High);
                }
            }
        }

        private void InitAdvImages()
        {
            string[] tmp = (from _tmp_ in asm.GetManifestResourceNames()
                            where _tmp_.Contains("Snake.Advs")
                            select _tmp_).ToArray();

            AllAdvBitmaps = new SKBitmap[tmp.Length][];

            for (int i = 0; i < tmp.Length; i++)
            {
                AllAdvBitmaps[i] = SplitBitmap(tmp[i]);
            }
        }
    }
}
