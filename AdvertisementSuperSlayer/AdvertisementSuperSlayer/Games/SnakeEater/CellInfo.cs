using SkiaSharp;
using System;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    class CellInfo
    {
        public ElementState State { get; set; }
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }

        public int OrderNum { get; set; }

        public bool WasEaten { get; set; }

        public SKBitmap Bmp { get; set; }
    }
}
