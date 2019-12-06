using SQLite;

namespace AdvertisementSuperSlayer.Helpers
{
    public interface ISQLiteProvider
    {
        SQLiteConnection GetConnection();
    }
}
