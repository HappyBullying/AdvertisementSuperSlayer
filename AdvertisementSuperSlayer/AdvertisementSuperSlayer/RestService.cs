using AdvertisementSuperSlayer.Data;
using AdvertisementSuperSlayer.DbModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdvertisementSuperSlayer
{
    public class RestService
    {
        private HttpClient client;
        public ApplicationDbContext Db { get; private set; }


        private readonly string loginUrl = "http://192.168.0.105:5000/api/account/login";
        private readonly string registerUrl = "http://192.168.0.105:5000/api/account/register";
        private readonly string deviceInfoUrl = "http://192.168.0.105:5000/api/account/postdevicedata";
        private readonly string pairRecordUrl = "http://192.168.0.105:5000/api/rating/postpairdata";
        private readonly string puzzleRecordUrl = "http://192.168.0.105:5000/api/rating/postpuzzledata";
        private readonly string snakeRecordUrl = "http://192.168.0.105:5000/api/rating/postsnakedata";
        private readonly string getSnakeRecord = "http://192.168.0.105:5000/api/rating/getsnakedata";
        private readonly string getPuzzleRecord = "http://192.168.0.105:5000/api/rating/getpuzzledata";
        private readonly string getPairRecord = "http://192.168.0.105:5000/api/rating/getpairdata";
        public readonly string advUrl = "http://192.168.0.105:5000/advertisement/advertisement";


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
                string retContent = await response.Content.ReadAsStringAsync();
                var anon = new
                {
                    username = "",
                    email = "",
                    status = "",
                    message = "",
                    ReturnableId = ""
                };

                var AnonNew = JsonConvert.DeserializeAnonymousType(retContent, anon);
                usr.ReturnableUser = AnonNew.ReturnableId;
                Db.SaveUser(usr);
                await SendDeviceInfo(AnonNew.ReturnableId);
                return true;
            }
            else
            {
                return false;
            }

        }


        public async Task<bool> SendDeviceInfo(string ReturnableId)
        {
            var deviceInfo = new
            {
                ProcessorCount = Environment.ProcessorCount,
                OSVersion = Environment.OSVersion.ToString(),
                MachineName = Environment.MachineName,
                LTOTDevice = DateTime.UtcNow,
                PlayerId = ReturnableId
            };

            string jsonContent = JsonConvert.SerializeObject(deviceInfo);
            StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(deviceInfoUrl, data);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
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
                    userrole = "",
                    ReturnableUser = ""
                };
                var token = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), define);

                Token tk = new Token
                {
                    AccessToken = token.token,
                    ExpireDate = token.expiration,
                };
                usr.ReturnableUser = token.ReturnableUser;
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
                    userrole = "",
                    ReturnableUser = ""
                };
                var token = JsonConvert.DeserializeAnonymousType(await response.Content.ReadAsStringAsync(), define);

                Token tk = new Token
                {
                    AccessToken = token.token,
                    ExpireDate = token.expiration,
                };
                usr.ReturnableUser = token.ReturnableUser;
                Db.SaveUser(usr);
                Db.SaveToken(tk);
                return true;
            }
            else
            {
                return false;
            }
        }



        public async void UpdatePair(PairRecord record)
        {
            bool needsUpdate = Db.SavePairRating(record);

            if (needsUpdate)
            {
                object toSend = new
                {
                    PairPlayerRatingId = 0,
                    GameTime = record.GameTime,
                    FailsNumber = record.Errors,
                    LastModified = DateTime.UtcNow,
                    PlayerId = Db.GetUser().ReturnableUser
                };
                string jsonContent = JsonConvert.SerializeObject(toSend);
                StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(pairRecordUrl, data);
            }
        }



        public async Task<List<PuzzleRecord>> GetPuzzleData()
        {
            HttpResponseMessage msg = await client.GetAsync(getPuzzleRecord);
            if (msg.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strConent = await msg.Content.ReadAsStringAsync();
                List<PuzzleRecord> records = JsonConvert.DeserializeObject<List<PuzzleRecord>>(strConent);
                return records;
            }
            return null;
        }





        public async Task<List<PairRecord>> GetPairData()
        {
            HttpResponseMessage msg = await client.GetAsync(getPairRecord);
            if (msg.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strConent = await msg.Content.ReadAsStringAsync();
                List<PairRecord> records = JsonConvert.DeserializeObject<List<PairRecord>>(strConent);
                return records;
            }
            return null;
        }



        public async Task<List<SnakeRecord>> GetSnakeData()
        {
            HttpResponseMessage msg = await client.GetAsync(getSnakeRecord);
            if (msg.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string strConent = await msg.Content.ReadAsStringAsync();
                List<SnakeRecord> records = JsonConvert.DeserializeObject<List<SnakeRecord>>(strConent);
                return records;
            }
            return null;
        }
            



        public async void UpdateSnake(SnakeRecord record)
        {
            bool needsUpdate = Db.SaveSnakeRating(record);

            if (needsUpdate)
            {
                object toSend = new
                {
                    SnakeRtId = 0,
                    MaxScore = record.MaxScore,
                    GameTime = record.GameTime,
                    LastModified = DateTime.UtcNow,
                    PlayerId = Db.GetUser().ReturnableUser
                };
                string jsonContent = JsonConvert.SerializeObject(toSend);
                StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(snakeRecordUrl, data);
            }
        }



        public async void UpdatePuzzle(PuzzleRecord record)
        {
            bool needsUpdate = Db.SavePuzzleRating(record);

            if (needsUpdate)
            {
                object toSend = new
                {
                    PuzzlePlayerRatingId = 0,
                    GameTime = record.GameTime,
                    LastModified = record.LastModified,
                    PlayerId = Db.GetUser().ReturnableUser
                };
                string jsonContent = JsonConvert.SerializeObject(toSend);
                StringContent data = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(puzzleRecordUrl, data);
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
