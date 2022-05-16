using GooseboardAPI.Code;
using GooseBoardMobile.Classes;
using Microsoft.AspNetCore.SignalR;
using MySqlConnector;

namespace SignalRChat.Hubs
{
    public class GooseHub : Hub
    {
        public static List<Users> Users = new List<Users>();
        //public bool connected { get; set; }
        //public new MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        //{
        //    Server = "gooseboarddb.ngrok.io",
        //    UserID = "daanjeu_king",
        //    Password = "Poepchinees1",
        //    Database = "daanjeu_dbdaan",
        //};


        public async Task SendMessage(string user, string message)
        {

            //Deserialize
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(message);

            //Check for diffrent codes
            if (obj.Code == "GOOSECODE_JOIN")
            {
                Users.Add(new Users
                {
                    UserID = obj.Sender,
                    Username = obj.User
                }) ;
            }
            if (obj.Code == "GOOSECODE_GETUSERS")
            {
                obj.Other = Newtonsoft.Json.JsonConvert.SerializeObject(Users);
            }

            //Seralizes back
            message = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        //public void GetDatabase(string query)
        //{
        //    MySqlConnection connection = new MySqlConnection(builder.ConnectionString);

        //}
    }
}