using luxe.Server.Application.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;
using luxe.Server.Infrastructure.Configurations;

namespace luxe.Server.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                throw new InvalidOperationException("Failed to send email.", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }

        }
    }
}
