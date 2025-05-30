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
    public class TestimonialService : BaseDatabaseService<IrisContext>, ITestimonialService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly string? _uploadPath;

        public TestimonialService(IrisContext context
            , IConfiguration configuration
            , IFileService fileService) : base(context)
        {
            _configuration = configuration;
            _fileService = fileService;
            _uploadPath = _configuration["UploadPath"];
        }

        public async Task<BaseResponse<List<TestimonialDTO>>> GetAllAsync(BaseRequest request)
        {
            return await HandlePaginatedActionAsync(async () =>
            {
                var query = _context.Testimonials
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Text))
                {
                    query = query.Where(x =>
                        x.ClientName.Contains(request.Text));
                }

                query = query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn);

                return await GetPaginatedResultAsync(query, request.PageNumber, request.PageSize);
            }, entity => new TestimonialDTO
            {
                Id = entity.Id,
                Comment = entity.Comment,
                ClientName = entity.ClientName,
                ClientOccupation = entity.ClientOccupation,
                ImagePath = entity.ImageRelativePath != null ? _configuration["ImageUrl"] + entity.ImageRelativePath : null
            });
        }

        public async Task<BaseResponse<TestimonialDTO?>> GetByIdAsync(int id)
        {
            return await HandleActionAsync(async () =>
            {
                var dbEntity = await _context.Testimonials
                    .AsNoTracking()
                    .Where(x => x.Id == id && x.IsActive)
                    .FirstOrDefaultAsync();

                if (dbEntity == null)
                {
                    InitMessageResponse("NotFound");
                    return null;
                }

                return new TestimonialDTO
                {
                    Id = dbEntity.Id,
                    Comment = dbEntity.Comment,
                    ClientName = dbEntity.ClientName,
                    ClientOccupation = dbEntity.ClientOccupation,
                    ImagePath = dbEntity.ImageRelativePath != null ? _configuration["ImageUrl"] + dbEntity.ImageRelativePath : null
                };
            });
        }

        public async Task<BaseResponse<Task>> AddAsync(int userId, TestimonialAddDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                if (await IsDuplicateAsync<Testimonial>(x =>
                x.ClientName == request.ClientName
                && x.ClientOccupation == request.ClientOccupation
                && x.Comment == request.Comment)) return;

                var dbEntity = new Testimonial
                {
                    Comment = request.Comment,
                    ClientName = request.ClientName,
                    ClientOccupation = request.ClientOccupation,
                };

                if (request.Image != null)
                {
                    var image = await _fileService.UploadAsync(_uploadPath, request.Image);
                    dbEntity.ImageRelativePath = image;
                }

                await _context.Testimonials.AddAsync(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> UpdateAsync(int userId, TestimonialUpdateDTO request)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await 
                    DoesNotExistAsync<Testimonial>(x => x.Id == request.Id);
                
                if (doesNotExist) return;

                dbEntity.Comment = request.Comment ?? dbEntity.Comment;
                dbEntity.ClientName = request.ClientName ?? dbEntity.ClientName;
                dbEntity.ClientOccupation = request.ClientOccupation ?? dbEntity.ClientOccupation;

                if (request.Image != null)
                {
                    if (!string.IsNullOrEmpty(dbEntity.ImageRelativePath))
                    {
                        _fileService.DeleteFile(_uploadPath, dbEntity.ImageRelativePath);
                    }

                    var image = await _fileService.UploadAsync(_uploadPath, request.Image);

                    dbEntity.ImageRelativePath = image;
                }

                _context.Testimonials.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }

        public async Task<BaseResponse<Task>> DeleteAsync(int userId, int id)
        {
            return await HandleVoidActionAsync(async () =>
            {
                var (doesNotExist, dbEntity) = await
                    DoesNotExistAsync<Testimonial>(x => x.Id == id);

                if (doesNotExist) return;

                if (!string.IsNullOrEmpty(dbEntity.ImageRelativePath))
                {
                    _fileService.DeleteFile(_uploadPath, dbEntity.ImageRelativePath);
                }

                dbEntity.IsActive = false;
                dbEntity.ImageRelativePath = null;

                _context.Testimonials.Update(dbEntity);
                await _context.SaveChangesAsync(userId);
            });
        }
    }
}
