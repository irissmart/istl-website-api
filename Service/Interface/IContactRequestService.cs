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
    public interface IContactRequestService
    {
        Task<BaseResponse<Task>> AddAsync(ContactRequestAddDTO contactRequestAddDTO);

        Task<BaseResponse<List<ContactRequestDTO>>> GetAllAsync(BaseRequest? request);

        Task<BaseResponse<ContactRequestDTO?>> GetByIdAsync(int id);
    }
}
