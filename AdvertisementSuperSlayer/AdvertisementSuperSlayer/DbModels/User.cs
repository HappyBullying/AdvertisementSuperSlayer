using System.ComponentModel.DataAnnotations;

namespace AdvertisementSuperSlayer.DbModels
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public User() { }
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
