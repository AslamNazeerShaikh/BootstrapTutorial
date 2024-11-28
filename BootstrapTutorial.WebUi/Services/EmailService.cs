using BootstrapTutorial.WebUi.Helpers;
using MailKit.Net.Smtp;
using MimeKit;

namespace BootstrapTutorial.WebUi.Services
{
    public class EmailService : IEmailService
    {
        private readonly ConfigHelper _configHelper;

        public EmailService(ConfigHelper configHelper)
        {
            _configHelper = configHelper;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body, string cc, string bcc, string attachmentPath)
        {
            var emailSettings = _configHelper.GetEmailSettings();

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(emailSettings.DisplayName, emailSettings.Email));
            emailMessage.To.Add(new MailboxAddress("", recipientEmail));

            if (!string.IsNullOrEmpty(cc))
                emailMessage.Cc.Add(new MailboxAddress("", cc));

            if (!string.IsNullOrEmpty(bcc))
                emailMessage.Bcc.Add(new MailboxAddress("", bcc));

            emailMessage.Subject = subject;

            var builder = new BodyBuilder
            {
                TextBody = body
            };

            if (!string.IsNullOrEmpty(attachmentPath))
                builder.Attachments.Add(attachmentPath);

            emailMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(emailSettings.Host, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailSettings.Email, emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
