using Framework.Model;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IContactService
    {
        Task<BaseResponse<ContactDTO?>> GetAsync();
        Task<BaseResponse<Task>> UpdateAsync(int userId, ContactDTO request);
    }
}
