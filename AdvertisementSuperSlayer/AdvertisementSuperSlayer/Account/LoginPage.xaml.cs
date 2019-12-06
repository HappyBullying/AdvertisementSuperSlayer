using AdvertisementSuperSlayer.DbModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private bool usernameReady = false;
        private bool passwordReady = false;
        public LoginPage()
        {
            InitializeComponent();
            googleBtn.Source = ImageSource.FromResource(App.PathToImages + "Common.google_logo.png");
        }

        private async void sbmButton_Clicked(object sender, EventArgs e)
        {
            User usr = new User
            {
                Username = username.Text,
                Password = password.Text
            };

            bool success = await App.Rest.Login(usr);

            if (!success)
            {
                await DisplayAlert("Login", "Invalid details supplied", "");
            }
            else
            {
                await Navigation.PushAsync(new Games.GameSelectMenue());
            }
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (username.Text.Length == 0)
            {
                usernameReady = false;
            }
            else
            {
                usernameReady = true;
            }
            ActivateButton();
        }

        private void password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (password.Text.Length == 0)
            {
                passwordReady = false;
            }
            else
            {
                passwordReady = true;
            }
            ActivateButton();
        }

        private void ActivateButton()
        {
            if (passwordReady && usernameReady)
            {
                sbmButton.IsEnabled = true;
            }
            else
            {
                sbmButton.IsEnabled = false;
            }
        }

        private void googleBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {

        }

        private void StackLayout_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}