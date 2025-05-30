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
    public interface IVisitorStatsService
    {
        Task<BaseResponse<Task>> AddAsync(VisitLogDto dto, string? ip);
        Task<BaseResponse<VisitStatsDto>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
