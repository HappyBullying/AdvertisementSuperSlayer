using AdvertisementSuperSlayer.Data;
using AdvertisementSuperSlayer.DbModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementSuperSlayer
{
    public class RestService
    {
        private HttpClient client;
        public ApplicationDbContext Db { get; private set; }


        private readonly string loginUrl = "http://192.168.0.105:5000/api/account/login";
        private readonly string registerUrl = "http://192.168.0.105:5000/api/account/register";

        public RestService()
        {
            client = new HttpClient();
            Db = new ApplicationDbContext();
        }


        public async Task<bool> Register(User usr, string email)
        {
            object toSend = new
            {
                Username = usr.Username,
                Password = usr.Password,
                Email = email
            };

            string jsonContent = JsonConvert.SerializeObject(toSend);
            StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(registerUrl, data);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Db.SaveUser(usr);
                return true;
            }
            else
            {
                return false;
            }

        }


        public async Task<bool> Login(User usr)
        {

            object toSend = new
            {
                Username = usr.Username,
                Password = usr.Password
            };
            string jsonContent = JsonConvert.SerializeObject(toSend);
            StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(loginUrl, data);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var define = new
                {
                    token = "",
                    expiration = DateTime.Now,
                    username = "",
                    userrole = ""
                };
                var token = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), define);

                Token tk = new Token
                {
                    AccessToken = token.token,
                    ExpireDate = token.expiration,
                };
                Db.SaveUser(usr);
                Db.SaveToken(tk);
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> Login()
        {
            User usr = Db.GetUser();

            object toSend = new
            {
                Username = usr.Username,
                Password = usr.Password
            };
            string jsonContent = JsonConvert.SerializeObject(toSend);
            StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(loginUrl, data);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var define = new
                {
                    token = "",
                    expiration = DateTime.Now,
                    username = "",
                    userrole = ""
                };
                var token = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), define);

                Token tk = new Token
                {
                    AccessToken = token.token,
                    ExpireDate = token.expiration,
                };

                Db.SaveToken(tk);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                Token tk = Db.GetToken();
                if (tk == null)
                    return false;

                if (tk.ExpireDate > DateTime.UtcNow)
                {
                    return true;
                }

                return false;
            }
        }
    }
}
