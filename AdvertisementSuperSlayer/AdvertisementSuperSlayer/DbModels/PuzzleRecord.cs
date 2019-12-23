using SQLite;
using System;

namespace AdvertisementSuperSlayer.DbModels
{
    public class PuzzleRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PuzzlePlayerRatingId { get; set; }

        public TimeSpan GameTime { get; set; }

        public DateTime LastModified { get; set; }
    }
}
