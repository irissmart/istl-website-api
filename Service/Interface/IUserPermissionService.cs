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
    public interface IUserPermission
    {
        Task<BaseResponse<List<dynamic>>> GetAllAsync(BaseRequest request);
        Task<BaseResponse<List<dynamic>>> GetAllByUserIdAsync(int userId, BaseRequest request);
    }
}
