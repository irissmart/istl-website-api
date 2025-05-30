using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IUserService
    {
        Task<BaseResponse<UserDTO?>> GetByIdAsync(int id);
        Task<BaseResponse<Task>> UpdateAsync(int userId, UserUpdateDTO request);
        Task<BaseResponse<Task>> ForgetPasswordAsync(string email);
        Task<BaseResponse<Task>> VerifyTokenAsync(string token);
        Task<BaseResponse<Task>> ResetPasswordAsync(string password, string token);
        Task<BaseResponse<Task>> ChangePasswordAsync(int id, ChangePasswordDTO request);
    }
}
