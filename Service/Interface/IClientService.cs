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
    public interface IClientService
    {
        Task<BaseResponse<List<ClientResponseDTO>>> GetAllAsync(BaseRequest request);

        Task<BaseResponse<ClientResponseDTO?>> GetByIdAsync(int id);

        Task<BaseResponse<Task>> AddAsync(int userId, ClientAddDTO request);

        Task<BaseResponse<Task>> UpdateAsync(int userId, ClientUpdateDTO request);

        Task<BaseResponse<Task>> DeleteAsync(int userId, int id);
    }
}
