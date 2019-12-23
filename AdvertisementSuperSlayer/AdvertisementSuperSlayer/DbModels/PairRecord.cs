using SQLite;
using System;

namespace AdvertisementSuperSlayer.DbModels
{
    public class PairRecord
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public TimeSpan GameDuration { get; set; }
        public DateTime RecordSetDate { get; set; }
        public int Errors { get; set; }
    }
}
