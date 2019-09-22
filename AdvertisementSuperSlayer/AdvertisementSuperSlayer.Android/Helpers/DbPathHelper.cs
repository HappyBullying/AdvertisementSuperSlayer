using AdvertisementSuperSlayer.Helpers;
using Xamarin.Forms;
using AdvertisementSuperSlayer.Droid.Helpers;
using System.IO;

[assembly: Dependency(typeof(DbPathHelper))]
namespace AdvertisementSuperSlayer.Droid.Helpers
{
    public class DbPathHelper : IDbPath
    {
        public DbPathHelper() { }
        public string GetDatabasePath(string filename)
        {
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), filename);
            return path;
        }
    }
}