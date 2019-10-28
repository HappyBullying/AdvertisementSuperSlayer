using System;

namespace AdvertisementSuperSlayer.Games.Pair.Views
{
    class ConfigEventArgs : EventArgs
    {
        public int FRow { get; set; }
        public int FCol { get; set; }
        public int SRow { get; set; }
        public int SCol { get; set; }
    }
}
