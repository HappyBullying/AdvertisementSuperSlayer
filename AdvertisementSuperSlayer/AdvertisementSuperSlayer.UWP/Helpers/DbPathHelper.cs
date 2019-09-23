using AdvertisementSuperSlayer.Helpers;
using AdvertisementSuperSlayer.UWP.Helpers;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(DbPathHelper))]
namespace AdvertisementSuperSlayer.UWP.Helpers
{
    public class DbPathHelper: IDbPath
    {
        public DbPathHelper() { }

        public string GetDatabasePath(string filename)
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
            return path;
        }
    }
}
