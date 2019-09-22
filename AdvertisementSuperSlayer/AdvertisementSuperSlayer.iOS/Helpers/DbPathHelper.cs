using System;
using System.IO;
using AdvertisementSuperSlayer.Helpers;
using AdvertisementSuperSlayer.iOS.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(DbPathHelper))]
namespace AdvertisementSuperSlayer.iOS.Helpers
{
    public class DbPathHelper : IDbPath
    {
        public DbPathHelper() { }

        public string GetDatabasePath(string filename)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", filename);
            return path;
        }
    }
}