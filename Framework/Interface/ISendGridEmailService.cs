using Framework.Model;

namespace Framework.Interface
{
    public interface ISendGridEmailService
    {
        Task<BaseResponse<bool>> SendEmail(string subject, string content, string toEmail, string toName);
    }
}