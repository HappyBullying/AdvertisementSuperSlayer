using SkiaSharp;
using System;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text;
using AdvertisementSuperSlayer.DbModels;

namespace AdvertisementSuperSlayer.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private const int STROKE_WIDTH = 50;
        private float offset;
        private float gradientCycleLength;
        private bool isAnimating;

        private SKRect pathBounds;
        private SKPath infinityPath;
        private SKColor[] colors;
        private Stopwatch stopwatch;

        public RegisterPage()
        {
            InitializeComponent();
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            object toSend = new
            {
                Username = username.Text,
                Email = email.Text,
                Password = password.Text
            };
            string jsonContent = JsonConvert.SerializeObject(toSend);
            StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            string url = "http://10.192.214.177:5000/api/account/register";
            HttpResponseMessage response = await client.PostAsync(url, data);


            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                await DisplayAlert("Login Error", await response.Content.ReadAsStringAsync(), response.StatusCode.ToString());
            }
            else
            {
                await DisplayAlert("Login", "Success", response.StatusCode.ToString());
                User usr = new User
                {
                    Username = username.Text,
                    Password = password.Text,
                };
                
                //if (App.AppDbContext.Users.Count(u => u.Username == username.Text) == 0)
                //{
                //    await App.AppDbContext.Users.AddAsync(usr);
                //}

                await Navigation.PushAsync(new Games.GameSelectMenue());
            }
        }

        private void cpassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cpassword.Text == password.Text)
            {
                sbmButton.IsEnabled = true;
            }
            else
            {
                sbmButton.IsEnabled = false;
            }
        }

        private void password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (password.Text.Length == 0)
            {
                sbmButton.IsEnabled = false;
            }
            else
            {
                sbmButton.IsEnabled = true;
            }
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (email.Text.Length == 0)
            {
                sbmButton.IsEnabled = false;
            }
            else
            {
                sbmButton.IsEnabled = true;
            }
        }

        private void username_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (username.Text.Length == 0)
            {
                sbmButton.IsEnabled = false;
            }
            else
            {
                sbmButton.IsEnabled = true;
            }
        }
    }
}