using Microsoft.AspNetCore.SignalR;

namespace therapy_app_backend.DAL.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string sender, string receiver, string message)
        {
            await Clients.User(receiver).SendAsync("ReceivePrivateMessage", sender, message);
        }
    }
}
