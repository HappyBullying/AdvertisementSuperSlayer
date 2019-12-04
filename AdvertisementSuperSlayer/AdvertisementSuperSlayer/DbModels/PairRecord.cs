using System;
using System.ComponentModel.DataAnnotations;

namespace AdvertisementSuperSlayer.DbModels
{
    public class PairRecord
    {
        [Key]
        public int Id { get; set; }

        public TimeSpan GameDuration { get; set; }
        public DateTime RecordSetDate { get; set; }
        public int Errors { get; set; }
    }
}
