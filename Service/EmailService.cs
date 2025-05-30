using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Service.Interface;

namespace Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var section = _configuration.GetSection("EmailSettings");
            var smtpHost = section["SmtpHost"] ?? throw new InvalidOperationException("Missing SmtpHost");
            var smtpPort = int.Parse(section["SmtpPort"] ?? "0");
            var userName = section["UserName"] ?? throw new InvalidOperationException("Missing UserName");
            var fromName = section["FromName"] ?? throw new InvalidOperationException("Missing Password");
            var password = section["Password"] ?? throw new InvalidOperationException("Missing Password");
            var enableSsl = bool.Parse(section["EnableSsl"] ?? "true");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, userName));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpHost, smtpPort, enableSsl
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(userName, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
