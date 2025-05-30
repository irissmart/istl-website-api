using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class DashboardService : BaseDatabaseService<IrisContext>, IDashboardService
    {
        public DashboardService(IrisContext context) : base(context)
        {
        }

        public async Task<BaseResponse<StatsDTO>> GetAllAsync()
        {
            return await HandleActionAsync(async () =>
            {
                var totalJobs = await _context.Jobs
                    .Where(x => x.IsActive && x.IsEnabled == true)
                    .CountAsync();

                var appliedJobs = await _context.JobApplications
                    .Where(x => x.IsActive)
                    .Distinct()
                    .CountAsync();

                var totalVisitors = await _context.VisitLogs
                    .CountAsync();

                var statsDto = new StatsDTO
                {
                    TotalJobs = totalJobs,
                    AppliedJobs = appliedJobs,
                    TotalVisitors = totalVisitors
                };

                return statsDto;
            });
        }
    }
}
