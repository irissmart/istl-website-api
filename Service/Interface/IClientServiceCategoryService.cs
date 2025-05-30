using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IClientServiceCategoryService
    {
        Task<BaseResponse<List<ClientServiceCategoryDTO>>> GetAllAsync(BaseRequest request);
        Task<BaseResponse<ClientServiceCategoryDTO?>> GetByIdAsync(int id);
        Task<BaseResponse<Task>> AddAsync(int userId, ClientServiceCategoryAddDTO request);
        Task<BaseResponse<Task>> UpdateAsync(int userId, ClientServiceCategoryUpdateDTO request);
        Task<BaseResponse<Task>> DeleteAsync(int userId, int id);
    }
}
