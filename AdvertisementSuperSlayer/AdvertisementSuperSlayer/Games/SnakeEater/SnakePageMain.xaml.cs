using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.SnakeEater
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SnakePageMain : ContentPage
    {
        private double tileSize;
        private readonly int Rows, Cols;
        private SKBitmap[][] advBitmaps;
        private SnakeDirection snakeDirection;
        private SKColor EmptyCellColor;
        private SKPaint OutCircle;
        private SKPaint InnerCircle;
        private SnakeField snakeField;
        public SnakePageMain(int rows, int cols)
        {
            InitializeComponent();
            snakeField = new SnakeField(rows, cols);
            snakeField.Margin = new Thickness(5);
            Content = snakeField;
            snakeField.InvalidateSurface();
        }


    }
}