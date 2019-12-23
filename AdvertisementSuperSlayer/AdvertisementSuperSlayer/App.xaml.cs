using Xamarin.Forms;
using AdvertisementSuperSlayer.DbModels;
using AdvertisementSuperSlayer.Account;
using Xamarin.Auth;

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
            Browser.BrowserPage page = new Browser.BrowserPage();
            page.Navigate("http://192.168.0.105:5000/home/login");
            OAuth2Authenticator authenticator = new OAuth2Authenticator(
               "666426372827-mfsinhpd7km8lf7uoqal65c15tsnm8f1.apps.googleusercontent.com",
               null,
               "https://www.googleapis.com/auth/userinfo.email",
               new System.Uri("https://accounts.google.com/o/oauth2/auth"),
               new System.Uri("https://vk.com"),
               new System.Uri("https://www.googleapis.com/oauth2/v4/token"),
               null, true);
            ///var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            //presenter.Login(authenticator);
            //MainPage = page; //new Games.SnakeEater.SnakePageMain(20, 32);
            MainPage = new Games.Puzzle.PuzzlePage(6);
            //MainPage = new NavigationPage(new LoginPage()) ; //new Games.SnakeEater.SnakePageMain(20, 32); //
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
