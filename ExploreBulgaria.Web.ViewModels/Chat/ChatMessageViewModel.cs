namespace ExploreBulgaria.Web.ViewModels.Chat
{
    public class ChatMessageViewModel
    {
        public string Name { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public DateTime SentOn { get; set; }

        public string Message { get; set; } = null!;
    }
}
