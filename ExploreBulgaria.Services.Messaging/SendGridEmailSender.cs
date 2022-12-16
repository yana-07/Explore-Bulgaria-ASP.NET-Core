using ExploreBulgaria.Services.Exceptions;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using static ExploreBulgaria.Services.Constants.MessageConstants;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Messaging
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridClient client;
        private readonly ILogger<SendGridEmailSender> logger;

        public SendGridEmailSender(
            string apiKey,
            ILogger<SendGridEmailSender> logger)
        {
            this.client = new SendGridClient(apiKey);
            this.logger = logger;
        }

        public async Task SendEmailAsync(
            string from,
            string fromName, 
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment>? attachments = null)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException(MustProvideSubjectAndContent);
            }

            var fromAddress = new EmailAddress(from, fromName);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);
            if (attachments?.Any() == true)
            {
                foreach (var attachment in attachments)
                {
                    message.AddAttachment(attachment.fileName,
                        Convert.ToBase64String(attachment.Content),attachment.mimeType);
                }
            }

            try
            {
                var response = await client.SendEmailAsync(message);
                logger.LogInformation(ResponseStatusCode, response.StatusCode);
                logger.LogInformation(ResponseBody, await response.Body.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaException(EmailSenderException, ex);
            }
        }
    }
}
