using AdvertisementSuperSlayer.DbModels;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AdvertisementSuperSlayer.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private bool usernameReady = false;
        private bool passwordReady = false;

        private const int STROKE_WIDTH = 50;
        private float offset;
        private float gradientCycleLength;
        private bool isAnimating;

        private SKRect pathBounds;
        private SKPath infinityPath;
        private SKColor[] colors;
        private Stopwatch stopwatch;

        public LoginPage()
        {
            InitializeComponent();
            BindingContext = this;
            googleBtn.Source = ImageSource.FromResource(App.PathToImages + "Common.google_logo.png");


            colors = new SKColor[3];
            colors[0] = SKColor.Parse("FFC3A0");
            colors[1] = SKColor.Parse("FFAFBD");
            colors[2] = SKColor.Parse("FFC3A0");


            infinityPath = new SKPath();
            infinityPath.MoveTo(0, 0);
            infinityPath.LineTo(1000f, 0);
            infinityPath.LineTo(1000f, 1000f);
            infinityPath.LineTo(0, 1000f);
            infinityPath.LineTo(0, 0);

            sbmButton.IsEnabled = false;

            // Calculate path information 
            pathBounds = infinityPath.Bounds;
            gradientCycleLength = pathBounds.Width +
                pathBounds.Height * pathBounds.Height / pathBounds.Width;
        }







        /*************************************************************************************************************/
        /* LOGIC START*/
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
                await Navigation.PushAsync(new MainPage());
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

        public ICommand RegisterNavigateCommand => new Command(async () =>
        {
            if (Navigation.NavigationStack.Count > 1 && Navigation.NavigationStack.ElementAt(Navigation.NavigationStack.Count - 2) is RegisterPage)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PushAsync(new RegisterPage());
            }
        });
        /* LOGIC END */
        /*************************************************************************************************************/






        /*************************************************************************************************************/
        /* FOR BACKGROUND ANIMATION START*/
        protected override void OnAppearing()
        {
            base.OnAppearing();
            isAnimating = true;
            stopwatch = new Stopwatch();
            stopwatch.Start();
            Device.StartTimer(TimeSpan.FromMilliseconds(35), OnTimerTick);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            isAnimating = false;
            stopwatch.Stop();
        }

        private void StackLayout_SizeChanged(object sender, EventArgs e)
        {
            StackLayout st = sender as StackLayout;
            double side = (Width - 370) / 2.0d;
            st.Margin = new Thickness(side, 0, side, 15);
        }

        private void canvasView_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            //Set transforms to shift path to center and scale to canvas size
            canvas.Translate(info.Width / 2, info.Height / 2);
            canvas.Scale(0.95f *
                Math.Min(info.Width / (pathBounds.Width + STROKE_WIDTH),
                         info.Height / (pathBounds.Height + STROKE_WIDTH)));



            using (SKPaint paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = STROKE_WIDTH;
                paint.Shader = SKShader.CreateLinearGradient(
                                    new SKPoint(pathBounds.Left, pathBounds.Top),
                                    new SKPoint(pathBounds.Right, pathBounds.Bottom),
                                    colors,
                                    null,
                                    SKShaderTileMode.Repeat,
                                    SKMatrix.MakeTranslation(offset, 0));

                canvas.DrawPaint(paint);
            }
        }

        private bool OnTimerTick()
        {
            const int duration = 2;     // seconds
            double progress = stopwatch.Elapsed.TotalSeconds % duration / duration;
            offset = (float)(gradientCycleLength * progress);
            canvasView.InvalidateSurface();

            return isAnimating;
        }
        /* FOR BACKGROUND ANIMATION END*/
        /*************************************************************************************************************/
    }
}