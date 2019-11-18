using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using SkiaSharp;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    class ImageGroupManager
    {
        private int indCount = -1;
        public int ImageRows { get; private set; }
        public int ImageCols { get; private set; }
        public List<int[]> Collection { get; private set; }

        private Assembly asm;
        private Stream stream;
        private int _sqr;
        public ImageGroupManager(int imageRows, int imageCols, int sqr)
        {
            ImageRows = imageRows;
            ImageCols = imageCols;
            Collection = new List<int[]>();
            asm = GetType().GetTypeInfo().Assembly;
            _sqr = sqr;
        }

        public int NextIndex
        {
            get
            {
                indCount++;
                return indCount;
            }
        }

        /// <summary>
        /// Adds new image range
        /// </summary>
        public void AddImages(int first)
        {
            int[] tmpInds = new int[ImageRows * ImageCols];
            for (int i = 0; i < tmpInds.Length; i++)
            {
                tmpInds[i] = i + first;
            }
            Collection.Add(tmpInds);
        }

        /// <summary>
        /// Removes image range starting form first element
        /// </summary>
        public void Remove(int first)
        {
            int toRemove = Collection.FindIndex(pred => pred[0] == first);
            Collection.RemoveAt(toRemove);
        }

        public  SnakeAdvBitmap[] SplitAdvImage(string fileName)
        {
            stream = asm.GetManifestResourceStream(fileName);
            SKBitmap fullBmp = SKBitmap.Decode(stream);
            SnakeAdvBitmap[] imageADS = new SnakeAdvBitmap[ImageRows * ImageCols];

            for (int row = 0; row < ImageRows; row++)
            {
                for (int col = 0; col < ImageCols; col++)
                {
                    SKBitmap bitmap = new SKBitmap(_sqr, _sqr);
                    SKRect dest = new SKRect(0, 0, _sqr, _sqr);
                    SKRect source = new SKRect(col * _sqr, row * _sqr, (col + 1) * _sqr, (row + 1) * _sqr);

                    using (SKCanvas canvas = new SKCanvas(bitmap))
                    {
                        canvas.DrawBitmap(fullBmp, source, dest);
                    }

                    imageADS[row * ImageCols + col] = (SnakeAdvBitmap)bitmap;
                }
            }
            return imageADS;
        }
    }
}
