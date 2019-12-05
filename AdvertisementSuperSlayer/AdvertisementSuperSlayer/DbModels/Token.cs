using SQLite;
using System;

namespace AdvertisementSuperSlayer.DbModels
{
    public class Token
    {
        [PrimaryKey]
        public int TokenId { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public Token() { }
    }
}
