using Microsoft.AspNetCore.SignalR;
using MySqlConnector;

namespace SignalRChat.Hubs
{
    public class GooseHub : Hub
    {
        public bool connected { get; set; }
        public new MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
        {
            Server = "gooseboarddb.ngrok.io",
            UserID = "daanjeu_king",
            Password = "Poepchinees1",
            Database = "daanjeu_dbdaan",
        };

        
        public async Task SendMessage(string user, string message)
        {
            if (message.Contains("DB"))
            {
                GetDatabase();
            }

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public void GetDatabase()
        {
            MySqlConnection connection = new MySqlConnection(builder.ConnectionString);

        }
    }
}