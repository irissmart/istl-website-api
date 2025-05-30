using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface INotificationService
    {
        Task<BaseResponse<List<NotificationDTO>>> GetAllAsync(int? userId, BaseRequest dto);
        Task<BaseResponse<Task>> AddAsync(int userId, NotificationAddDTO request);
        Task<BaseResponse<Task>> UpdateAsync(int userId);
    }
}
