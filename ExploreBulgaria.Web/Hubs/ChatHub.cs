using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Web.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace ExploreBulgaria.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IAdminService adminService;
        private readonly IChatService chatService;

        public ChatHub(
            IAdminService adminService,
            IChatService chatService)
        {
            this.adminService = adminService;
            this.chatService = chatService;
        }

        public async Task SendMessageToGroup(string message, string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                groupName = $"private@{Context.UserIdentifier}";
            }

            string user = $"{Context.User!.FirstName()} {Context.User!.LastName()}";
            var userIdentifier = Context.UserIdentifier;
            var dateTime = DateTime.UtcNow;
            var avatar = Context.User!.AvatarUrl();

            if (Context.User!.Email() != "adminuser@abv.bg")
            {
                await adminService.NotifyAdmin(groupName);
                await chatService.SendChat(Context.UserIdentifier!, message, dateTime);
            }
         
            await Clients.Group(groupName)
                .SendAsync("ReceiveMessage", user, userIdentifier , avatar, message, dateTime);
        }

        public async Task JoinPrivateGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                groupName = $"private@{Context.UserIdentifier}";
            }
            
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
