using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Model;
using Service.DTOs.Response;

namespace Service.Interface
{
    public interface IDashboardService
    {
        Task<BaseResponse<StatsDTO>> GetAllAsync();
    }
}
