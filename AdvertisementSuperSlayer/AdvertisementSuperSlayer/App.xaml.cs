using System;
using System.Linq;
using Xamarin.Forms;
using System.Net.Http;
using Xamarin.Forms.Xaml;
using AdvertisementSuperSlayer.Data;
using AdvertisementSuperSlayer.DbModels;
using AdvertisementSuperSlayer.Helpers;
using Newtonsoft.Json;
using System.Text;
using SQLite;
using AdvertisementSuperSlayer.Account;

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
            //MainPage = new NavigationPage(new Account.RegisterPage());
            StartupNavigation();
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
                MainPage = new NavigationPage(new Games.GameSelectMenue());
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
