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
    public interface ITeamMemberService
    {
        Task<BaseResponse<List<TeamMemberDTO>>> GetAllAsync(BaseRequest request);

        Task<BaseResponse<TeamMemberDTO?>> GetByIdAsync(int id);

        Task<BaseResponse<Task>> AddAsync(int userId, TeamMemberAddDTO request);

        Task<BaseResponse<Task>> UpdateAsync(int userId, TeamMemberUpdateDTO request);

        Task<BaseResponse<Task>> DeleteAsync(int userId, int id);
    }
}
