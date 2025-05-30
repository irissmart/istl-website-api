using Framework.Model;
using Service.DTOs.Request;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface ITagService
    {
        Task<BaseResponse<List<TagDTO>>> GetAllAsync(BaseRequest request);
        Task<BaseResponse<Task>> AddAsync(int userId, string tagName);
        Task<BaseResponse<Task>> UpdateAsync(int userId, TagUpdateDTO request);
    }
}
