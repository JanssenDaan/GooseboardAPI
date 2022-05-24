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
            if (obj.Code == "GOOSECODE_LOGIN")
            {
                obj.Reciever = obj.Sender;
            }
            //Seralizes back
            message = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
   
}