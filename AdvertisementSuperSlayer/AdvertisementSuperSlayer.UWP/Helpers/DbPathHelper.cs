using AdvertisementSuperSlayer.Helpers;
using System.IO;
using Windows.Storage;

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
