using System;
using AdvertisementSuperSlayer.Helpers;
using Xamarin.Forms;
using System.IO;
using AdvertisementSuperSlayer.macOS.Helpers;
using SQLite;

[assembly: Dependency(typeof(DbProvider))]
namespace AdvertisementSuperSlayer.macOS.Helpers
{
    public class DbProvider : ISQLiteProvider
    {
        public DbProvider() { }

        public SQLiteConnection GetConnection()
        {
            string fileName = "ASSDB.db3";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
            SQLiteConnection conn = new SQLiteConnection(path);
            return conn;
        }

        public string GetDatabasePath(string filename)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
            return path;
        }
    }
}
