using System;
using AdvertisementSuperSlayer.Helpers;
using Xamarin.Forms;
using System.IO;
using AdvertisementSuperSlayer.macOS.Helpers;

[assembly: Dependency(typeof(DbPathHelper))]
namespace AdvertisementSuperSlayer.macOS.Helpers
{
    public class DbPathHelper : IDbPath
    {
        public DbPathHelper() { }

        public string GetDatabasePath(string filename)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
            return path;
        }
    }
}
