using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.Puzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PuzzlePage : ContentPage
    {
        private int Rows;
        private int Cols;
        public PuzzlePage(int rows, int cols)
        {
            InitializeComponent();
            Rows = rows;
            Cols = cols;
        }
    }
}