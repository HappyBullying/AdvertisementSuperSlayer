using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertisementSuperSlayer.Games.Snake
{
    class Advertisement
    {
        private List<SKBitmap> advs = new List<SKBitmap>();
        Random rnd;
        public Advertisement()
        {
            rnd = new Random(DateTime.Now.Millisecond);
            advs.Add(BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.tank.png"));
            advs.Add(BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.long_expan.png"));
            advs.Add(BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.mops.png"));
        }

        public SKBitmap Next
        {
            get
            {
                int generated = rnd.Next(advs.Count);
                SKImageInfo sKImageInfo = new SKImageInfo(190, 72);
                return advs[generated].Resize(sKImageInfo, SKFilterQuality.High);
            }
        }
    }
}
