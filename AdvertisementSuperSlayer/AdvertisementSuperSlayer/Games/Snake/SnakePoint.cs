using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertisementSuperSlayer.Games.Snake
{
    class SnakePoint
    {
        public Enum Direction { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        
        public SnakePoint() { }
        
        public SnakePoint(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
