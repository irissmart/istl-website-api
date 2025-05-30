using Framework.Interface;
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
    public class JobCategoryService : BaseDatabaseService<IrisContext>, IJobCategoryService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;

        public JobCategoryService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<BaseResponse<List<JobCategoryDTO>>> GetAllAsync(BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.JobCategories
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Text))
                {
                    query = query.Where(x => x.JobCategoryName.Contains(request.Text));
                }

                query = query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new JobCategoryDTO
            {
                Id = entity.Id,
                JobCategoryName = entity.JobCategoryName,
                ImagePath = entity.ImageRelativePath != null ? _configuration["ImageUrl"] + entity.ImageRelativePath : null
            });
        }

        public async Task<BaseResponse<JobCategoryDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.JobCategories
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new JobCategoryDTO
                {
                    Id = dbEntity.Id,
                    JobCategoryName = dbEntity.JobCategoryName,
                    ImagePath = dbEntity.ImageRelativePath != null ? _configuration["ImageUrl"] + dbEntity.ImageRelativePath : null
                };
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, JobCategoryAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (await IsDuplicateAsync<JobCategory>(x =>
                x.JobCategoryName == request.JobCategoryName)) return;

                var dbEntity = new JobCategory
                {
                    JobCategoryName = request.JobCategoryName,
                };

                if (request.Image != null)
                {
                    var image = await _fileService.UploadAsync(_uploadPath, request.Image);
                    dbEntity.ImageRelativePath = image;
                }

                await _context.JobCategories.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, JobCategoryUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await
                    DoesNotExistAsync<JobCategory>(x => x.Id == request.Id);

                if (doesNotExist) return;

                dbEntity.JobCategoryName = request.JobCategoryName ?? dbEntity.JobCategoryName;

                if (request.Image != null)
                {
                    if (!string.IsNullOrEmpty(dbEntity.ImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, dbEntity.ImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.Image);

                    dbEntity.ImageRelativePath = image;
                }

                _context.JobCategories.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> DeleteAsync(int userId, int id)
        {
            return await HandleVoidActionAsync(async () =>
             {
                 var dbEntity = await _context.JobCategories
                     .Include(x => x.Jobs.Where(x => x.IsActive))
                     .Where(x => x.Id == id && x.IsActive)
                     .FirstOrDefaultAsync();

                 if (dbEntity == null)
                 {
                     InitMessageResponse("NotFound");
                     return;
                 }

                 dbEntity.IsActive = false;
                 
                 foreach(var job in dbEntity.Jobs)
                 {
                     job.IsActive = false;
                 }

                 _context.JobCategories.Update(dbEntity);
                 await _context.SaveChangesAsync(userId);
             });
        }
    }
}
