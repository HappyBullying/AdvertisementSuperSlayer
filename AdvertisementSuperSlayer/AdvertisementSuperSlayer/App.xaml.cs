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
            AppDbContext = new ApplicationDbContext(dbPath);

            MainPage = new NavigationPage(new Account.RegisterPage());
            //StartupNavigation();
        }

        private async void StartupNavigation()
        {
            if (AppDbContext.database.Table<User>().Count() > 0)
            {
                User usr = AppDbContext.database.Table<User>().FirstOrDefault();
                HttpClient client = new HttpClient();
                object toSend = new
                {
                    Username = usr.Username,
                    Password = usr.Password
                };
                string jsonContent = JsonConvert.SerializeObject(toSend);
                StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                string url = "http://localhost:5000/api/account/login";
                HttpResponseMessage response = await client.PostAsync(url, data);

                
                var definition = new { token = "", expiration = DateTime.Now, username = "", userrole = "" };
                var desAnon = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), definition);
                Token tk = new Token
                {
                    AccessToken = desAnon.token,
                    ExpireDate = desAnon.expiration
                };
                AppDbContext.database.Table<Token>().Delete();
                //AppDbContext.database.Table<Token>().
                //MainPage = new NavigationPage(new Games.GameSelectMenue());
            }
            else
            {
                MainPage = new NavigationPage(new Account.RegisterPage());
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
