using System;
using System.ComponentModel.DataAnnotations;

namespace AdvertisementSuperSlayer.DbModels
{
    public class Token
    {
        [Key]
        public int TokenId { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public Token() { }
    }
}
