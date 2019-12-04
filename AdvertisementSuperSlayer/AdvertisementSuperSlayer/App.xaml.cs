using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AdvertisementSuperSlayer.DbModels;
using AdvertisementSuperSlayer.Helpers;

namespace AdvertisementSuperSlayer
{
    public partial class App : Application
    {
        public const string DBFILENAME = "ASSDB.db";
        public static readonly string PathToImages = "AdvertisementSuperSlayer.Images.";

        public static Data.ApplicationDbContext AppDbContext { get; private set; }

        public App()
        {
            InitializeComponent();
            string dbPath = DependencyService.Get<IDbPath>().GetDatabasePath(DBFILENAME);
            AppDbContext = new Data.ApplicationDbContext(dbPath);
            AppDbContext.Database.EnsureCreated();
            MainPage = new Account.RegisterPage(); //new NavigationPage(new MainPage()); 
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
