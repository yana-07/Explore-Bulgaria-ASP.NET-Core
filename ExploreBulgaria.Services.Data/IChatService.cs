using ExploreBulgaria.Web.ViewModels.Chat;

namespace ExploreBulgaria.Services.Data
{
    public interface IChatService
    {
        Task SendChat(string fromId, string message, DateTime sentOn);

        Task<IEnumerable<ChatMessageViewModel>> GetMessages(string fromId, string toId);

        Task ClearMessages(ClearChatMessageViewModel model);
    }
}
