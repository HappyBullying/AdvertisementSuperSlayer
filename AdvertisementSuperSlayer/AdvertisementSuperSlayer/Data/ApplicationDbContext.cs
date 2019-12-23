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
            dbConnection.CreateTable<PairRecord>();
            dbConnection.CreateTable<PuzzleRecord>();
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



        public bool SavePuzzleRating(PuzzleRecord record)
        {
            lock (locker)
            {
                PuzzleRecord old = dbConnection.Table<PuzzleRecord>().FirstOrDefault();
                if (old == null)
                {
                    dbConnection.Insert(record);
                    return true;
                }
                else
                {
                    if (record.GameTime < old.GameTime)
                    {
                        record.PuzzlePlayerRatingId = old.PuzzlePlayerRatingId;
                        dbConnection.Update(record, typeof(PuzzleRecord));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }


        public bool SaveSnakeRating(SnakeRecord record)
        {
            lock(locker)
            {
                SnakeRecord old = dbConnection.Table<SnakeRecord>().FirstOrDefault();

                if (old == null)
                {
                    dbConnection.Insert(record);
                    return true;
                }
                else
                {
                    if (record.MaxScore > old.MaxScore)
                    {
                        record.SnakeRtId = old.SnakeRtId;
                        dbConnection.Update(record, typeof(SnakeRecord));
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public bool SavePairRating(PairRecord record)
        {
            lock(locker)
            {
                PairRecord old = dbConnection.Table<PairRecord>().FirstOrDefault();
                if (old == null)
                {
                    dbConnection.Insert(record);
                    return true;
                }
                else
                {
                    if (record.Errors < old.Errors)
                    {
                        record.Id = old.Id;
                        dbConnection.Update(record, typeof(PairRecord));
                        return true;
                    }
                    else
                    {
                        if (record.Errors == old.Errors && (record.GameDuration < old.GameDuration))
                        {
                            record.Id = old.Id;
                            dbConnection.Update(record, typeof(PairRecord));
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }
    }
}
