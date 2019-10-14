using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using AdvertisementSuperSlayer.Games.Snake;

namespace AdvertisementSuperSlayer.Games.Snake
{
    class SnakeHeadsCollection
    {
        public SnakeHeadsCollection(byte delay)
        {
            Closed = BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.closed400x612.png");
            AClosed = BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.almost_closed400x612.png");
            Open = BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.open400x612.png");
            EOpen = BitmapExtensions.LoadBitmapResource(typeof(SnakePage), "AdvertisementSuperSlayer.Images.extra_open400x612.png");
            current = SnakeStage.Closed;
            current_img = Closed;
            this.Delay = delay;
            this._Delay = delay;
        }

        public SKBitmap Closed;
        public SKBitmap AClosed;
        public SKBitmap Open;
        public SKBitmap EOpen;
        
        private Enum current;
        private SKBitmap current_img;
        private byte Delay;
        private readonly byte _Delay;

        public void Reset()
        {
            current = SnakeStage.EOpen;
        }

        ///<summary>
        ///Get next image for animation
        ///</summary>
        public SKBitmap Next
        {
            get
            {
                if (Delay < 0)
                {
                    Delay--;
                    return current_img;
                }
                this.Delay = _Delay;
                if (current.CompareTo(SnakeStage.Open) < 0)
                {
                    if (current.CompareTo(SnakeStage.AClosed) == 0)
                    {
                        current = SnakeStage.Open;
                        current_img = this.Open;
                        return current_img;
                    }
                    else
                    {
                        current = SnakeStage.AClosed;
                        current_img = this.AClosed;
                        return current_img;
                    }
                }
                else
                {
                    if (current.CompareTo(SnakeStage.Open) == 0)
                    {
                        current = SnakeStage.EOpen;
                        current_img = this.EOpen;
                        return current_img;
                    }
                    else
                    {
                        current = SnakeStage.Closed;
                        current_img = this.Closed;
                        return current_img;
                    }
                }
            }
        }
    }
}
