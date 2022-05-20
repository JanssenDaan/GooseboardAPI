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
        public new MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "mysql.lamdev.be",
            UserID = "daanj",
            Password = "pFFx5menVQPJGY9mrNnXZ4v5PfJRFhSf",
            Database = "daanj_gooseboard",
        };


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
                obj.Other = GetUser(obj.Other);
                obj.Reciever = obj.Sender;  
            }
            //Seralizes back
            message = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        //public void GetDatabase(string query)
        //{
        //    MySqlConnection connection = new MySqlConnection(builder.ConnectionString);

        //}
        public string GetUser(string other)
        {
            string pswrd = "";
            var user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(other);
            MySqlConnection connection = new MySqlConnection(builder.ConnectionString);

            using (connection)
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = $"SELECT password FROM users WHERE username = '" + user.Password + "';";
                //    comm.Parameters.Add("?emaill", MySqlDbType.Text).Value = entryLoginEmail.Text;
                using (MySqlCommand command = new MySqlCommand(comm.CommandText, connection))
                {

                    object psw = command.ExecuteScalar();
                    if (psw != null)
                    {
                        pswrd = psw.ToString();
                    }
                }

            }
            if (pswrd == user.Password)
            {
                return "CORRECT";
            }
            else
            {
                return "INCORRECT";
            }
            return "";
        }
    }
   
}