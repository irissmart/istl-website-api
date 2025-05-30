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
    public interface IPartnerService
    {
        Task<BaseResponse<List<PartnerResponseDTO>>> GetAllAsync(BaseRequest request);

        Task<BaseResponse<PartnerResponseDTO?>> GetByIdAsync(int id);

        Task<BaseResponse<Task>> AddAsync(int userId, PartnerAddDTO request);

        Task<BaseResponse<Task>> UpdateAsync(int userId, PartnerUpdateDTO request);

        Task<BaseResponse<Task>> DeleteAsync(int userId, int id);
    }
}
