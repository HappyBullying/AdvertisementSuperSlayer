using SQLite;
using System;

namespace AdvertisementSuperSlayer.DbModels
{
    public class PuzzleRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PuzzlePlayerRatingId { get; set; }

        public long GameTime { get; set; }

        public string Username { get; set; }

        public DateTime LastModified { get; set; }
    }
}
