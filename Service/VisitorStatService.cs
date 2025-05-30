using System.Security.Cryptography;
using System.Text;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class VisitorStatService : BaseDatabaseService<IrisContext>, IVisitorStatsService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _uploadPath;
        public VisitorStatService(IrisContext context
            , IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
            _uploadPath = _configuration["UploadPath"];
        }


        public async Task<BaseResponse<Task>> AddAsync(VisitLogDto dto, string? ip)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(dto.PageUrl))
                {
                    InitMessageResponse("BadRequest", "Page URL is required.");
                    return;
                }

                var visitorFingerprint = GenerateVisitorFingerprint(
                    ip,
                    dto.UserAgent,
                    dto.ClientId);


                var isNewVisitor = !await _context.VisitLogs
                    .AnyAsync(v => v.VisitorFingerprint == visitorFingerprint &&
                                  v.VisitDate > DateTime.UtcNow.AddMonths(-3));

                var visitLog = new VisitLog
                {
                    PageUrl = dto.PageUrl.Trim(),
                    IpAddress = ip,
                    UserAgent = dto.UserAgent,
                    Referrer = dto.Referrer,
                    VisitDate = DateTime.UtcNow,
                    VisitorFingerprint = visitorFingerprint,
                    ClientId = dto.ClientId,
                    IsNewVisitor = isNewVisitor,
                    SessionId = dto.SessionId
                };

                await _context.VisitLogs.AddAsync(visitLog);
                await _context.SaveChangesAsync(0);
            });
        }

        private string GenerateVisitorFingerprint(string? ipAddress, string? userAgent, string? clientId)
        {
            using var sha = SHA256.Create();

            ipAddress ??= string.Empty;
            userAgent ??= string.Empty;
            clientId ??= string.Empty;

            var input = $"{ipAddress}-{userAgent}-{clientId}";
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }

        public async Task<BaseResponse<VisitStatsDto>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            return await HandleActionAsync(async () =>
            {
                startDate ??= DateTime.UtcNow.AddDays(-30); 
                endDate ??= DateTime.UtcNow;

                var stats = await _context.VisitLogs
                    .Where(v => v.VisitDate >= startDate && v.VisitDate <= endDate)
                    .GroupBy(v => 1)
                    .Select(g => new VisitStatsDto
                    {
                        TotalVisits = g.Count(),
                        UniqueVisitors = g.Select(v => v.VisitorFingerprint).Distinct().Count(),
                        NewVisitors = g.Count(v => v.IsNewVisitor),
                        PagesVisited = g.Select(v => v.PageUrl).Distinct().Count(),
                        AverageVisitsPerVisitor = (double)g.Count() / g.Select(v => v.VisitorFingerprint).Distinct().Count(),
                        StartDate = startDate,
                        EndDate = endDate
                    })
                    .FirstOrDefaultAsync();

                return stats ?? new VisitStatsDto
                {
                    TotalVisits = 0,
                    UniqueVisitors = 0,
                    NewVisitors = 0,
                    PagesVisited = 0,
                    AverageVisitsPerVisitor = 0,
                    StartDate = startDate,
                    EndDate = endDate
                };
            });
        }
    }
}