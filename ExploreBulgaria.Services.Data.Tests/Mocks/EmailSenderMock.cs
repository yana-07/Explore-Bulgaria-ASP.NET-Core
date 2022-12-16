using ExploreBulgaria.Services.Messaging;

namespace ExploreBulgaria.Services.Data.Tests.Mocks
{
    public class EmailSenderMock : IEmailSender
    {
        public Task SendEmailAsync(string from,
            string fromName, 
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment>? attachments = null)
        {
            return Task.CompletedTask;
        }
    }
}
