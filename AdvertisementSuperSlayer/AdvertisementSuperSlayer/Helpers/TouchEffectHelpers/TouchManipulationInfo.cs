using System;

using SkiaSharp;

namespace AdvertisementSuperSlayer.Helpers.TouchEffectHelpers
{
    class TouchManipulationInfo
    {
        public SKPoint PreviousPoint { set; get; }

        public SKPoint NewPoint { set; get; }
    }
}
