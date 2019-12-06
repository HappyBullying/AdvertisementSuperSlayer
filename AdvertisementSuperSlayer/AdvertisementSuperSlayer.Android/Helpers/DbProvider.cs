using AdvertisementSuperSlayer.Helpers;
using Xamarin.Forms;
using AdvertisementSuperSlayer.Droid.Helpers;
using System.IO;
using SQLite;

[assembly: Dependency(typeof(DbProvider))]
namespace AdvertisementSuperSlayer.Droid.Helpers
{
    public class DbProvider : ISQLiteProvider
    {
        public DbProvider() { }

        public SQLiteConnection GetConnection()
        {
            string fileName = "ASSDB.db3";
            string path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
            SQLiteConnection conn = new SQLiteConnection(path);
            return conn;
        }

    }
}