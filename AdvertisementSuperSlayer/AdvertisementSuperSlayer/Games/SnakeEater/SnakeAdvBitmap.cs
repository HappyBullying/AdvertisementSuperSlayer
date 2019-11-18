using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    class SnakeAdvBitmap : SKBitmap
    {
        public int OrderNumber { get; set; }
        public int ColIndex { get; set; }
        public int RowIndex { get; set; }
        public bool WasEaten { get; set; }

        public SnakeAdvBitmap() { }

        
    }
}
