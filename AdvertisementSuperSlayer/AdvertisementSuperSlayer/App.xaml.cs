using Xamarin.Forms;
using AdvertisementSuperSlayer.DbModels;
using AdvertisementSuperSlayer.Account;
using Xamarin.Auth;
using System.Collections.Generic;

namespace AdvertisementSuperSlayer
{
    public partial class App : Application
    {
        public static readonly string PathToImages = "AdvertisementSuperSlayer.Images.";
        public static RestService Rest { get; private set; }


        public App()
        {
            InitializeComponent();
            Rest = new RestService();
            //StartupNavigation();

            //MainPage = new NavigationPage(new Games.Puzzle.PuzzlePage(6));
            MainPage = new Page1();
        }

        private void StartupNavigation()
        {
            // islogged in must check refresh token
            if (!Rest.IsLoggedIn)
            {
                User usr = Rest.Db.GetUser();

                if (usr != null)
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    MainPage = new NavigationPage(new RegisterPage());
                }
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
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
