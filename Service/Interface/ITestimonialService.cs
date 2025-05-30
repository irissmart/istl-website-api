using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface ITestimonialService
    {
        Task<BaseResponse<List<TestimonialDTO>>> GetAllAsync(BaseRequest request);
        Task<BaseResponse<TestimonialDTO?>> GetByIdAsync(int id);
        Task<BaseResponse<Task>> AddAsync(int userId, TestimonialAddDTO request);
        Task<BaseResponse<Task>> UpdateAsync(int userId, TestimonialUpdateDTO request);
        Task<BaseResponse<Task>> DeleteAsync(int userId, int id);
    }
}
