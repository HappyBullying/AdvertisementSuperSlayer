using System;

using SkiaSharp;

namespace AdvertisementSuperSlayer.TouchTracking.Transforms
{
    class TouchManipulationInfo
    {
        public SKPoint PreviousPoint { set; get; }

        public SKPoint NewPoint { set; get; }
    }
}
