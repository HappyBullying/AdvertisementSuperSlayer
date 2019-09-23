using System;
using System.ComponentModel.DataAnnotations;

namespace AdvertisementSuperSlayer.DbModels
{
    public class FirstTime
    {
        [Key]
        public int Id { get; set; }

        public DateTime FirstEnter { get; set;  }
    }
}
