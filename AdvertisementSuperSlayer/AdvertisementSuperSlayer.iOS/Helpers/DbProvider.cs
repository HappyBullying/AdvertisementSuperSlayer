using System;
using System.IO;
using AdvertisementSuperSlayer.Helpers;
using AdvertisementSuperSlayer.iOS.Helpers;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(DbProvider))]
namespace AdvertisementSuperSlayer.iOS.Helpers
{
    public class DbProvider : ISQLiteProvider
    {
        public DbProvider() { }


        SQLiteConnection ISQLiteProvider.GetConnection()
        {
            string fileName = "ASSDB.db3";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", fileName);
            SQLiteConnection conn = new SQLiteConnection(path);
            return conn;
        }
    }
}