using SQLite;
using System;

namespace AdvertisementSuperSlayer.DbModels
{
    public class SnakeRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        public int SnakeRtId { get; set; }

        public int MaxScore { get; set; }

        public string Username { get; set; }

        public long GameTime { get; set; }

        public DateTime LastModified { get; set; }
    }
}
