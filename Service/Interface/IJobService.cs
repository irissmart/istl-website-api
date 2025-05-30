using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IJobService
    {
        Task<BaseResponse<List<JobDTO>>> GetAllAsync(BaseRequest request, int? categoryId, bool? isEnabled, bool? isAdmin);
        Task<BaseResponse<List<JobLookupDTO>>> GetLookupAsync(BaseRequest request, int? categoryId);
        Task<BaseResponse<JobDTO?>> GetByIdAsync(int id);
        Task<BaseResponse<Task>> AddAsync(int userId, JobAddDTO request);
        Task<BaseResponse<Task>> UpdateAsync(int userId, JobUpdateDTO request);
        Task<BaseResponse<Task>> UpdateStatusAsync(int userId, JobStatusUpdateDTO request);
        Task<BaseResponse<Task>> DeleteAsync(int userId, int id);
    }
}
