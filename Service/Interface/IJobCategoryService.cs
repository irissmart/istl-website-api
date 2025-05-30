using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IJobCategoryService
    {
        Task<BaseResponse<List<JobCategoryDTO>>> GetAllAsync(BaseRequest request);
        Task<BaseResponse<JobCategoryDTO?>> GetByIdAsync(int id);
        Task<BaseResponse<Task>> AddAsync(int userId, JobCategoryAddDTO request);
        Task<BaseResponse<Task>> UpdateAsync(int userId, JobCategoryUpdateDTO request);
        Task<BaseResponse<Task>> DeleteAsync(int userId, int id);

    }
}
