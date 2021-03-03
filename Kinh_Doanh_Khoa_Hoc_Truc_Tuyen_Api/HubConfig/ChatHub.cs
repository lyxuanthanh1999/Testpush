using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.HubConfig
{
    public class ChatHub : Hub
    {
        private static readonly ConnectionMapping<string> Connections = new ConnectionMapping<string>();

        public async Task SendToUserAsync(string userId, AnnouncementViewModel message)
        {
            foreach (var connectionId in Connections.GetConnections(userId))
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
        }

        public Task SendMessage(AnnouncementViewModel message)
        {
            return Clients.All.SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToGroup(string group, AnnouncementViewModel message)
        {
            return Clients.Group(group).SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            Connections.Add(userId, Context.ConnectionId);
            if (Context.User.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "CheckOrder");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var userId = Context.UserIdentifier;
            Connections.Remove(userId, Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}