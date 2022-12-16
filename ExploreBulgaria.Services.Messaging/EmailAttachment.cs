namespace ExploreBulgaria.Services.Messaging
{
    public class EmailAttachment
    {
        public byte[] Content { get; set; } = null!;

        public string fileName { get; set; } = null!;

        public string mimeType { get; set; } = null!;
    }
}
