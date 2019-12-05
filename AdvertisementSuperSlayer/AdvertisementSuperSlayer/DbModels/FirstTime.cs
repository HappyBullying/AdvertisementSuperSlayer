using SQLite;
using System;

namespace AdvertisementSuperSlayer.DbModels
{
    public class FirstTime
    {
        [PrimaryKey]
        public int Id { get; set; }

        public DateTime FirstEnter { get; set;  }

        public bool AutoLogin { get; set; }
    }
}
