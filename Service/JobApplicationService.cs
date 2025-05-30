using Framework.Interface;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Constants;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Enums;
using Service.Interface;

namespace Service
{
    public class JobApplicationService : BaseDatabaseService<IrisContext>, IJobApplicationService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;
        private readonly INotificationService _notificationService;


        public JobApplicationService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService
            , INotificationService notificationService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
            _notificationService = notificationService;
        }

        public async Task<BaseResponse<JobApplicationDetailDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.JobApplications
                    .AsNoTracking()
                    .Include(x => x.Job)
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new JobApplicationDetailDTO
                {
                    Id = id,
                    JobId = dbEntity.JobId,
                    JobName = dbEntity.Job.Title,
                    FullName = $"{dbEntity.FirstName} {dbEntity.LastName}",
                    Email = dbEntity.Email,
                    Contact = dbEntity.Contact,
                    Message = dbEntity.Message,
                    CreatedOn = dbEntity.CreatedOn,
                    FileRelativePath = _configuration["ImageUrl"] + dbEntity.FileRelativePath
                };
            });
        }

        public async Task<BaseResponse<List<JobApplicationDTO>>> GetAllAsync(int? jobCategoryId, int? jobId, BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.JobApplications
                    .Include(x => x.Job)
                    .ThenInclude(x => x.JobCategory)
                    .AsQueryable();

                if(jobCategoryId != null)
                {
                    query = query.Where(x => x.Job.JobCategory.Id  == jobCategoryId);
                }

                if (jobId != null)
                {
                    query = query.Where(x => x.Job.Id == jobId);
                }

                query = query.OrderByDescending(x => x.CreatedOn);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new JobApplicationDTO
            {
                Id = entity.Id,
                JobId = entity.JobId,
                JobName = entity.Job.Title,
                FullName = $"{entity.FirstName} {entity.LastName}",
                Email = entity.Email,
                Contact = entity.Contact,
                CreatedOn = entity.CreatedOn
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(JobApplicationAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (await IsDuplicateAsync<JobApplication>(x =>
                x.JobId == request.JobId && x.Contact == request.Contact)) return;

                var job = await _context.Jobs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.JobId
                                            && x.ExpiresOn > DateTime.UtcNow
                                            && x.JobStatusId == (int)Enums.JobStatus.Active
                                            && x.IsEnabled == true
                                            && x.IsActive);

                if (job == null)
                {
                    InitMessageResponse("BadRequest", "Job not found.");
                    return;
                }

                var dbEntity = new JobApplication
                {
                    JobId = request.JobId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Contact = request.Contact,
                    Message = request.Message,
                };

                if (request.Document != null)
                {
                    var document = await _fileService.UploadAsync(_uploadPath, request.Document);
                    dbEntity.FileRelativePath = document;
                }

                await _context.JobApplications.AddAsync(dbEntity);

                var recieverId = await _context.Users
                                                .Where(x => (x.UserRoleId == (int)Enums.UserRole.Admin && x.IsVerified && x.IsActive))
                                                .Select(x => x.Id)
                                                .FirstOrDefaultAsync();

                var message = Helper.Interpolate(Notifications.JobApplicationReceived, new[] { $"{dbEntity.FirstName} {dbEntity.LastName}",  job.Title});

                var notificationDTO = new NotificationAddDTO
                {
                    UserId = recieverId,
                    Message = message,
                    NotificationType = Enums.NotificationType.JobApplication,
                };

                await _notificationService.AddAsync(0, notificationDTO);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int id)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await
                    DoesNotExistAsync<JobApplication>(x => x.Id == id);

                if (dbEntity == null) return;

                dbEntity.IsActive = false;

                _context.JobApplications.Update(dbEntity);
                await _context.SaveChangesAsync(0);
            });
        }
    }
}
