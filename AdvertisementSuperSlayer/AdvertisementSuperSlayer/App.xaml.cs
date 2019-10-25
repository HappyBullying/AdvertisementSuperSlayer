using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Games.Pair.Views.PairCards(); //new Games.Snake.SnakePage(); //new Games.Bitmap.Bmppage();//new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
