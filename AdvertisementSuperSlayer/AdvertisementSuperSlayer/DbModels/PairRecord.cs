using SQLite;
using System;

namespace AdvertisementSuperSlayer.DbModels
{
    public class PairRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public long GameTime { get; set; }
        public DateTime LasModified { get; set; }

        public string Username { get; set; }
        public int Errors { get; set; }
    }
}
