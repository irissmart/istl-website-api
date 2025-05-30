using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Interface;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class JobTagService : BaseDatabaseService<IrisContext>, IJobTagService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;

        public JobTagService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<BaseResponse<List<JobTagDTO>>> GetAllAsync(BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.JobTags
                    .AsQueryable();

                query = query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new JobTagDTO
            {
                Id = entity.Id,
                JobId = entity.JobId,
                TagId = entity.TagId,
            });
        }

        public async Task<BaseResponse<JobTagDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.JobTags
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new JobTagDTO
                {
                    Id = dbEntity.Id,
                    JobId = dbEntity.JobId,
                    TagId = dbEntity.TagId,
                };
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, JobTagAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (await IsDuplicateAsync<JobTag>(x =>
                x.JobId == request.JobId && x.TagId == request.TagId)) return;

                var dbEntity = new JobTag
                {
                    JobId = request.JobId,
                    TagId = request.TagId
                };

                await _context.JobTags.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }
    }
}