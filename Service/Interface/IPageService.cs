using Framework.Model;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IPageService
    {
        Task<BaseResponse<List<PageResponseDTO>>> GetAllAsync(BaseRequest dto);
    }
}
