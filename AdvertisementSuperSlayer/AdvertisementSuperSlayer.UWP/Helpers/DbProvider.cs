using AdvertisementSuperSlayer.Helpers;
using AdvertisementSuperSlayer.UWP.Helpers;
using SQLite;
using System.IO;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(DbProvider))]
namespace AdvertisementSuperSlayer.UWP.Helpers
{
    public class DbProvider: ISQLiteProvider
    {
        public DbProvider() { }

        public SQLiteConnection GetConnection()
        {
            string sqliteFilename = "ASSDB.db3";
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);
            SQLiteConnection conn = new SQLiteConnection(path);
            return conn;
        }
    }
}
