using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace AdvertisementSuperSlayer.GTK
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();

            App app = new App();
            FormsWindow window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("AdvertisementSuperSlayer");
            window.Show();

            Gtk.Application.Run();
        }
    }
}
