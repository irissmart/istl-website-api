using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IClientServiceService
    {
        Task<BaseResponse<ClientServiceDTO?>> GetByIdAsync(int id);
        Task<BaseResponse<List<ClientServiceDTO>>> GetAllFilteredAsync(int? categoryId, BaseRequest request);
        Task<BaseResponse<List<ClientServiceDTO>>> GetAllRelatedAsync(int id, string serviceName, BaseRequest request);
        Task<BaseResponse<Task>> AddAsync(int userId, ClientServiceAddDTO request);
        Task<BaseResponse<Task>> UpdateAsync(int userId, ClientServiceUpdateDTO request);
        Task<BaseResponse<Task>> DeleteAsync(int userId, int id);
    }
}
