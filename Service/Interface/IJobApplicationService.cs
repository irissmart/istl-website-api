using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IJobApplicationService
    {
        Task<BaseResponse<JobApplicationDetailDTO?>> GetByIdAsync(int id);
        Task<BaseResponse<List<JobApplicationDTO>>> GetAllAsync(int? jobCategoryId, int? jobId, BaseRequest request);
        Task<BaseResponse<Task>> AddAsync(JobApplicationAddDTO request);
        Task<BaseResponse<Task>> UpdateAsync(int id);
    }
}
