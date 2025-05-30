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
    public interface IJobTagService
    {
        Task<BaseResponse<List<JobTagDTO>>> GetAllAsync(BaseRequest request);

        Task<BaseResponse<JobTagDTO?>> GetByIdAsync(int id);

        Task<BaseResponse<Task>> AddAsync(int userId, JobTagAddDTO request);
    }
}
