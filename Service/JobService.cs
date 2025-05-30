using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Interface;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;

namespace Service
{
    public class JobService : BaseDatabaseService<IrisContext>, IJobService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;
        public JobService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<BaseResponse<List<JobDTO>>> GetAllAsync(BaseRequest request, int? categoryId, bool? isEnabled, bool? isAdmin)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.Jobs
                    .Include(x => x.JobCategory)
                    .Include(x => x.JobTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.JobStatus)
                    .Include(x => x.JobApplications.Where(x => x.IsActive))
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Text))
                {
                    query = query.Where(x => x.Title.Contains(request.Text));
                }

                if (categoryId != 0 && categoryId != null)
                {
                    query = query.Where(x => x.JobCategoryId == categoryId);
                }

                if (isEnabled != null)
                {
                    query = query.Where(x => x.IsEnabled == isEnabled);
                }

                if (isAdmin != null && isAdmin == false)
                {
                    query = query.Where(x => x.ExpiresOn > DateTime.UtcNow
                                            && x.JobStatusId == (int)Enums.JobStatus.Active
                                            && x.IsEnabled != false
                                            && x.IsActive);
                }

                query = query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new JobDTO
            {
                Id = entity.Id,
                JobCategoryId = entity.JobCategoryId,
                JobCategoryName = entity.JobCategory.JobCategoryName,
                JobCategoryImage = entity.JobCategory.ImageRelativePath != null ? _configuration["ImageUrl"] + entity.JobCategory.ImageRelativePath : null,
                Title = entity.Title,
                Currency = entity.Currency,
                MinSalary = entity.MinSalary,
                MaxSalary = entity.MaxSalary,
                JobStatusId = entity.JobStatusId,
                JobStatusName = entity.JobStatus.JobStatusName,
                JobTags = entity.JobTags.Where(x => x.IsActive).Select(x => new JobTagNameDTO { Id = x.Id, TagId = x.TagId, TagName = x.Tag.TagName }).ToList(),
                ExperienceYearsRequired = entity.ExperienceYearsRequired,
                Vacancies = entity.Vacancies,
                ExpiresOn = entity.ExpiresOn,
                Description = entity.Description,
                Responsibilities = entity.Responsibilities,
                City = entity.City,
                Country = entity.Country,
                TotalJobApplications = entity.JobApplications.Count(),
            });
        }

        public async Task<BaseResponse<List<JobLookupDTO>>> GetLookupAsync(BaseRequest request, int? categoryId)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.Jobs
                    .Where(x => x.IsActive && x.JobStatusId == (int)Enums.JobStatus.Active)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Text))
                {
                    query = query.Where(x => x.Title.Contains(request.Text));
                }

                if (categoryId != 0 && categoryId != null)
                {
                    query = query.Where(x => x.JobCategoryId == categoryId);
                }

                query = query.OrderBy(x => x.Title);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new JobLookupDTO
            {
                Id = entity.Id,
                Title = entity.Title,
            });
        }

        public async Task<BaseResponse<JobDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.Jobs
                    .AsNoTracking()
                    .Include(x => x.JobCategory)
                    .Include(x => x.JobTags)
                    .ThenInclude(x => x.Tag)
                    .Include(x => x.JobStatus)
                    .Include(x => x.JobApplications.Where(x => x.IsActive))
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new JobDTO
                {
                    Id = dbEntity.Id,
                    JobCategoryId = dbEntity.JobCategoryId,
                    JobCategoryName = dbEntity.JobCategory.JobCategoryName,
                    JobCategoryImage = dbEntity.JobCategory.ImageRelativePath != null ? _configuration["ImageUrl"] + dbEntity.JobCategory.ImageRelativePath : null,
                    Title = dbEntity.Title,
                    Currency = dbEntity.Currency,
                    MinSalary = dbEntity.MinSalary,
                    MaxSalary = dbEntity.MaxSalary,
                    JobStatusId = dbEntity.JobStatusId,
                    JobStatusName = dbEntity.JobStatus.JobStatusName,
                    JobTags = dbEntity.JobTags.Where(x => x.IsActive).Select(x => new JobTagNameDTO { Id = x.Id, TagId = x.TagId, TagName = x.Tag.TagName }).ToList(),
                    ExperienceYearsRequired = dbEntity.ExperienceYearsRequired,
                    Vacancies = dbEntity.Vacancies,
                    ExpiresOn = dbEntity.ExpiresOn,
                    Description = dbEntity.Description,
                    Responsibilities = dbEntity.Responsibilities,
                    City = dbEntity.City,
                    Country = dbEntity.Country,
                    TotalJobApplications = dbEntity.JobApplications.Count()
                };
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, JobAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntity = new Job
                {
                    JobCategoryId = request.JobCategoryId,
                    Title = request.Title,
                    Currency = request.Currency,
                    MinSalary = request.MinSalary,
                    MaxSalary = request.MaxSalary,
                    JobStatusId = request.JobStatusId,
                    ExperienceYearsRequired = request.ExperienceYearsRequired,
                    Vacancies = request.Vacancies,
                    ExpiresOn = request.ExpiresOn,
                    Description = request.Description,
                    Responsibilities = request.Responsibilities,
                    City = request.City,
                    Country = request.Country,
                    JobTags = (request.JobTags ?? new List<JobTagAddRequestDTO>())
                        .DistinctBy(x => x.TagId)
                        .Select(tag => new JobTag
                        {
                            TagId = tag.TagId
                        }).ToList()
                };

                await _context.Jobs.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, JobUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntity = await _context.Jobs
                    .Include(x => x.JobTags)
                    .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive);

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return;
                }

                dbEntity.Title = request.Title;
                dbEntity.Currency = request.Currency;
                dbEntity.MinSalary = request.MinSalary;
                dbEntity.MaxSalary = request.MaxSalary;
                dbEntity.JobStatusId = request.JobStatusId;
                dbEntity.ExperienceYearsRequired = request.ExperienceYearsRequired;
                dbEntity.Vacancies = request.Vacancies;
                dbEntity.ExpiresOn = request.ExpiresOn;
                dbEntity.Description = request.Description;
                dbEntity.Responsibilities = request.Responsibilities;
                dbEntity.City = request.City;
                dbEntity.Country = request.Country;

                var existingTags = dbEntity.JobTags.Where(x => x.IsActive).ToList();
                var requestedTagIds = request.JobTags.Select(x => x.TagId).Distinct().ToList();

                foreach (var existingTag in existingTags)
                {
                    if (!requestedTagIds.Contains(existingTag.TagId))
                    {
                        existingTag.IsActive = false;
                    }
                }

                foreach (var tagId in requestedTagIds)
                {
                    var exists = existingTags.Any(x => x.TagId == tagId);

                    if (!exists)
                    {
                        dbEntity.JobTags.Add(new JobTag
                        {
                            TagId = tagId,
                        });
                    }
                    else
                    {
                        var existingTag = existingTags.FirstOrDefault(x => x.TagId == tagId && !x.IsActive);

                        if (existingTag != null)
                        {
                            existingTag.IsActive = true;
                        }
                    }
                }

                _context.Jobs.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateStatusAsync(int userId, JobStatusUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await
                    DoesNotExistAsync<Job>(x => x.Id == request.JobId);

                if (doesNotExist) return;

                dbEntity.JobStatusId = request.JobStatusId;

                if (request.JobStatusId == (int)Enums.JobStatus.Active)
                {
                    dbEntity.IsEnabled = true;
                }
                else if (request.JobStatusId == (int)Enums.JobStatus.Inactive)
                {
                    dbEntity.IsEnabled = false;
                }

                _context.Jobs.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> DeleteAsync(int userId, int id)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var dbEntity = await _context.Jobs
                    .Include(x => x.JobTags.Where(x => x.IsActive))
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();
                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return;
                }
                dbEntity.IsActive = false;

                foreach (var jobTag in dbEntity.JobTags)
                {
                    jobTag.IsActive = false;
                }

                // We're not deleting job applications.

                _context.Jobs.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }
    }
}