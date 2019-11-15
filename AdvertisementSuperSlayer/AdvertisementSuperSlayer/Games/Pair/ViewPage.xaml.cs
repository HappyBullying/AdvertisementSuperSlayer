using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Games.Pair
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPage : ContentPage
    {
        int secs;
        public ViewPage()
        {
            InitializeComponent();
            //Img.Source = ImageSource.FromResource("AdvertisementSuperSlayer.Images.Pair.p2.png");
            ////Navigation.InsertPageBefore(new Puzzle.PuzzlePage(4, 4), this);
            
            secs = 3000; 
            StartTimer();
        }

        private void StartTimer()
        {
            TimeSpan inter = TimeSpan.FromMilliseconds(100);
            Device.StartTimer(inter, TimerTick);
        }

        private bool TimerTick()
        {
            int tmp1 = secs / 1000;
            int tmp2 = (secs % 1000) / 100;
            LB.Text = $"{tmp1}.{tmp2}";
            secs -= 100;
            if (secs != 0)
                return true;
            Navigation.PushAsync(new Puzzle.PuzzlePage(4, 4));
            return false;
        }
    }
}