using AdvertisementSuperSlayer.DbModels;
using AdvertisementSuperSlayer.Helpers;
using SQLite;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer.Data
{
    public class ApplicationDbContext
    {
        private string _dbPath;
        private SQLiteConnection dbConnection;
        private object locker = new object();


        public ApplicationDbContext()
        {
            dbConnection = DependencyService.Get<ISQLiteProvider>().GetConnection();
            dbConnection.CreateTable<User>();
            dbConnection.CreateTable<Token>();
            dbConnection.CreateTable<FirstTime>();
        }

        public User GetUser()
        {
            lock(locker)
            {
                if (dbConnection.Table<User>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return dbConnection.Table<User>().FirstOrDefault();
                }
            }
        }

        public int SaveUser (User usr)
        {
            lock (locker)
            {
                if (dbConnection.Table<User>().Count() != 0)
                {
                    usr.UserId = dbConnection.Table<User>().Where(u => u.Username == usr.Username).FirstOrDefault().UserId;
                    return dbConnection.Update(usr);
                }
                else
                {
                    return dbConnection.Insert(usr);
                }
            }
        }


        public Token  GetToken()
        {
            lock (locker)
            {
                if (dbConnection.Table<Token>().Count() == 0)
                {
                    return null;
                }
                else
                {
                    return dbConnection.Table<Token>().FirstOrDefault();
                }
            }
        }

        public int SaveToken(Token tk)
        {
            lock (locker)
            {
                dbConnection.DeleteAll<Token>();
                return dbConnection.Insert(tk);
            }
        }
    }
}
