using Framework.Configuration;
using Framework.Interface;
using Framework.Model;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Framework.Service
{
    public class SendGridEmailService : BaseService, ISendGridEmailService
    {
        protected readonly SendGridConfig _sendGridConfig;

        public SendGridEmailService(SendGridConfig sendGridConfig)
        {
            _sendGridConfig = sendGridConfig;
        }

        public async Task<BaseResponse<bool>> SendEmail(string subject, string content, string toEmail, string toName)
        {
            return await HandleActionAsync(async () =>
            {
                var client = new SendGridClient(_sendGridConfig.ApiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(_sendGridConfig.FromEmail, _sendGridConfig.FromName),
                    Subject = subject,
                    PlainTextContent = content
                };
                msg.AddTo(new EmailAddress(toEmail, toName));
                var response = await client.SendEmailAsync(msg);
                return response.IsSuccessStatusCode;
            });
        }
    }
}