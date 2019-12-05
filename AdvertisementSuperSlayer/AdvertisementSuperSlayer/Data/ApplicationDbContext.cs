using AdvertisementSuperSlayer.DbModels;
using SQLite;

namespace AdvertisementSuperSlayer.Data
{
    public class ApplicationDbContext
    {
        private string _dbPath;
        public SQLiteConnection database;


        public ApplicationDbContext(string dbPath)
        {
            this._dbPath = dbPath;
            database = new SQLiteConnection(dbPath);
            database.CreateTable<User>();
            database.CreateTable<Token>();
            database.CreateTable<FirstTime>();
        }

    }
}
