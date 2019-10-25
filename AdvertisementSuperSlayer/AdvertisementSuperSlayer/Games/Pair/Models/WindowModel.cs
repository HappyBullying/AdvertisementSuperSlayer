using Xamarin.Forms;
using System.Collections.Generic;
using AdvertisementSuperSlayer.Games.Pair.Views;

namespace AdvertisementSuperSlayer.Games.Pair.Models
{
    class WindowModel 
    {
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }


        public StackOrientation Orientation { get; set; }
        public List<PhotoHalfPairTile> Tiles { get; set; }
        public Image[] AdvImages { get; set; }
    }
}
